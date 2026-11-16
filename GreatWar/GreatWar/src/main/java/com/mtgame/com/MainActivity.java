package com.mtgame.com;

import static com.squareup.picasso.PicassoProvider.context;

import android.Manifest;
import android.annotation.SuppressLint;
import android.content.Context;
import android.content.ContextWrapper;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;

import com.alipay.sdk.app.AuthTask;

import com.alipay.sdk.app.PayTask;

import com.kwai.monitor.log.TurboAgent;
import com.kwai.monitor.log.TurboConfig;
import com.mtgame.com.aliapi.AuthResult;
import com.mtgame.com.aliapi.PayResult;
import com.mtgame.com.oaId.DemoHelper;
import com.taptap.sdk.core.TapTapRegion;
import com.taptap.sdk.kit.internal.callback.TapTapCallback;
import com.taptap.sdk.kit.internal.exception.TapTapException;
import com.taptap.sdk.login.Scopes;
import com.taptap.sdk.login.TapTapAccount;
import com.taptap.sdk.login.TapTapLogin;
import com.tencent.bugly.crashreport.CrashReport;
import com.unity3d.player.UnityPlayer;
import com.kwai.monitor.payload.TurboHelper;
import java.util.Map;
import android.os.Message;
import android.os.Handler;
import android.telephony.TelephonyManager;
import android.text.TextUtils;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.core.content.PermissionChecker;

import com.mtgame.com.playerActivity.UnityPlayerActivity;
import com.web.FloatWidgetHelper;
import com.web.MallState;
import com.web.interfaces.OnInitCallback;
import com.web.interfaces.WebViewCallback;

import com.taptap.sdk.core.TapTapSdk;
import com.taptap.sdk.core.TapTapSdkOptions;

public class MainActivity extends UnityPlayerActivity implements DemoHelper.AppIdsUpdater{
    public static String channelInfo = "";
    //阿里
    private static final int SDK_PAY_FLAG = 1;
    private static final int SDK_AUTH_FLAG = 2;

    private int MY_PERMISSIONS_REQUEST_READ_PHONE_STATE = 1;


    @Override
    protected void onCreate(Bundle savedIntanceState) {
        super.onCreate(savedIntanceState);
    }


    public void AliAuth(String serverGeneratedAuthInfo)
    {
        // authInfo 的构造方式详见 授权请求参数 一节，或参考支付宝 SDK Demo 中的实现。
// authInfo 的生成包括签名逻辑。故生成过程请务必在服务端进行。
//        final String authInfo = serverGeneratedAuthInfo;
// 对授权接口的调用需要异步进行。
        Runnable authRunnable = new Runnable() {
            @Override
            public void run() {
                // 构造AuthTask 对象
                AuthTask authTask = new AuthTask(MainActivity.this);
                // 调用授权接口
                // AuthTask#authV2(String info, boolean isShowLoading)，
                // 获取授权结果。
                Map<String, String> result = authTask.authV2(serverGeneratedAuthInfo, true);
                // 将授权结果以 Message 的形式传递给 App 的其它部分处理。
                // 对授权结果的处理逻辑可以参考支付宝 SDK Demo 中的实现。
                Message msg = new Message();
                msg.what = SDK_AUTH_FLAG;
                msg.obj = result;
                mHandler.sendMessage(msg);
            }
        };
        Thread authThread = new Thread(authRunnable);
        authThread.start();
    }

    public void AliPay(String orderInfo)
    {
        Runnable payRunnable = new Runnable() {
            @Override
            public void run() {
                PayTask alipay = new PayTask(MainActivity.this);
                Map <String,String> result = alipay.payV2(orderInfo,true);
                Message msg = new Message();
                msg.what = SDK_PAY_FLAG;
                msg.obj = result;
                mHandler.sendMessage(msg);
            }
        };
        // 必须异步调用
        Thread payThread = new Thread(payRunnable);
        payThread.start();

    }

    @SuppressLint("HandlerLeak")
    private Handler mHandler = new Handler() {
        @SuppressWarnings("unused")
        public void handleMessage(Message msg) {
            switch (msg.what) {
                case SDK_PAY_FLAG: {
                    @SuppressWarnings("unchecked")
                    PayResult payResult = new PayResult((Map<String, String>) msg.obj);
                    /**
                     * 对于支付结果，请商户依赖服务端的异步通知结果。同步通知结果，仅作为支付结束的通知。
                     */
                    String resultInfo = payResult.getResult();// 同步返回需要验证的信息
                    String resultStatus = payResult.getResultStatus();
                    // 判断resultStatus 为9000则代表支付成功
                    if (TextUtils.equals(resultStatus, "9000")) {
                        // 该笔订单是否真实支付成功，需要依赖服务端的异步通知。
                        UnityPlayer.UnitySendMessage("SDKManager", "AliPaySucc", resultInfo);
                    } else {
                        // 该笔订单真实的支付结果，需要依赖服务端的异步通知。
                        //showAlert(PayDemoActivity.this, getString(R.string.pay_failed) + payResult);
                        UnityPlayer.UnitySendMessage("SDKManager", "AliPayFail", "支付失败");
                    }
                    break;
                }
                case SDK_AUTH_FLAG: {
                    @SuppressWarnings("unchecked")
                    AuthResult authResult = new AuthResult((Map<String, String>) msg.obj, true);
                    String resultStatus = authResult.getResultStatus();

                    // 判断resultStatus 为“9000”且result_code
                    // 为“200”则代表授权成功，具体状态码代表含义可参考授权接口文档
                    if (TextUtils.equals(resultStatus, "9000") && TextUtils.equals(authResult.getResultCode(), "200")) {
                        // 获取alipay_open_id，调支付时作为参数extern_token 的value
                        // 传入，则支付账户为该授权账户
                        //showAlert(PayDemoActivity.this, getString(R.string.auth_success) + authResult);
                        UnityPlayer.UnitySendMessage("SDKManager", "AliAuthSucc", authResult.getResult());
                    } else {
                        // 其他状态值则为授权失败
                        Log.d("SdkDemo: AliAuthFail", "授权失败");
                        //showAlert(PayDemoActivity.this, getString(R.string.auth_failed) + authResult);
                        UnityPlayer.UnitySendMessage("SDKManager", "AliAuthFail", "授权失败");
                    }
                    break;
                }
                default:
                    break;
            }
        };
    };

    public void InitShop(String host, String appId, String token)
    {
        FloatWidgetHelper.init(host, MainActivity.this, appId, token, new OnInitCallback() {
            @Override
            public void onResult(MallState mallState, String s) {
                if (mallState == MallState.OPEN) {
                    UnityPlayer.UnitySendMessage("SDKManager", "ShowShopOpen", s);
                } else if (mallState == MallState.CLOSE) {
                    UnityPlayer.UnitySendMessage("SDKManager", "ShowShopClose", s);
                } else if (mallState == MallState.ERROR) {
                    UnityPlayer.UnitySendMessage("SDKManager", "ShowShopError", s);
                }
            }
        }, false, new WebViewCallback() {
            @Override
            public void onShow(boolean b) {
                UnityPlayer.UnitySendMessage("SDKManager", "ShowShopOpen", "");
            }

            @Override
            public void onHide(boolean b) {
                UnityPlayer.UnitySendMessage("SDKManager", "ShowShopClose", "");
            }
        },null);
    }

    public void ShowShop()
    {
        FloatWidgetHelper.openWeb(MainActivity.this);
    }

    public void HideShop() {
        FloatWidgetHelper.dismissIcon();
    }

    public void TapTapInit()
    {
        TapTapSdkOptions tapSdkOptions = new TapTapSdkOptions(
                "oufkqoy0dzioelfkwf", // 游戏 Client ID
                "5kuQpFPsXVyY4Fs9eiaVVaD8rWCWJYqfDHufh6aq", // 游戏 Client Token
                TapTapRegion.CN // 游戏可玩区域: [TapTapRegion.CN]=国内 [TapTapRegion.GLOBAL]=海外
        );
        tapSdkOptions.setEnableLog(false);
        // 初始化 TapSDK
        TapTapSdk.init(this, tapSdkOptions);
    }

    public void TapTapLogin()
    {
        TapTapAccount currentTapAccount = TapTapLogin.getCurrentTapAccount();

        if (currentTapAccount != null) {
            // 已登录
            UnityPlayer.UnitySendMessage("SDKManager", "TapTapLoginSucc", currentTapAccount.getOpenId());
        } else {
            // 定义授权范围
            String[] scopes = new String[]{Scopes.SCOPE_PROFILE};

            TapTapLogin.loginWithScopes(this, scopes, new TapTapCallback<TapTapAccount>() {
                @Override
                public void onSuccess(TapTapAccount tapTapAccount) {
                    // 登录成功
                    UnityPlayer.UnitySendMessage("SDKManager", "TapTapLoginSucc", tapTapAccount.getOpenId());
                }

                @Override
                public void onFail(@NonNull TapTapException exception) {
                    // 登录失败
                    UnityPlayer.UnitySendMessage("SDKManager", "TapTapLoginFail", "");
                }

                @Override
                public void onCancel() {
                    // 登录取消
                    UnityPlayer.UnitySendMessage("SDKManager", "TapTapLoginCancel", "");
                }
            });
        }

    }

    public void TapTapLoginOut()
    {
        TapTapLogin.logout();
    }

    //    1激活2注册3付费4回到游戏5切到后台6初始化sdk
    public void KuaiShou(int key, String value)
    {
        switch (key) {
            case 1:
                TurboAgent.onAppActive();
                break;
            case 2:
                TurboAgent.onRegister();
                break;
            case 3:
                TurboAgent.onPay(Double.parseDouble(value));
                break;
            case 4:
                TurboAgent.onPageResume(this);
                break;
            case 5:
                TurboAgent.onPagePause(this);
                break;
            case 6:
                //快手
                TurboAgent.init(TurboConfig.TurboConfigBuilder.create(this)
                        .setAppId("98352")
                        .setAppName("doulongzhanshi")
                        .setAppChannel(TurboHelper.getChannel(this))
                        .setEnableDebug(false)
                        .build());
                break;
        }
    }

    public void GetKuaiShouChannel()
    {
        String channel = TurboHelper.getChannel(this);
        UnityPlayer.UnitySendMessage("SDKManager", "GetKuaiShouChannel", channel);
    }

    public void InitAndroidOAID()
    {
        DemoHelper demoHelper = new DemoHelper(this);
        demoHelper.getDeviceIds(this);
    }

    @Override
    public void onIdsValid(String ids) {
        runOnUiThread(() -> {
            //tv_info.setText("OAID: \n" + ids);
            //tv_sys.setText(getSysInfo());
            UnityPlayer.UnitySendMessage("SDKManager", "GetAndroidOAID", ids);
        });
    }

    public void InitBugly()
    {
        ApplicationInfo appInfo = null;
        try {
            appInfo = getPackageManager().getApplicationInfo(getPackageName(), PackageManager.GET_META_DATA);
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
        String BUGLY_APPID = String.valueOf(appInfo.metaData.get("BUGLY_APPID"));
//        Log.d("SdkDemo: BUGLY_APPID", BUGLY_APPID);
        CrashReport.initCrashReport(getApplicationContext(), BUGLY_APPID, true);
    }

    public void TestJavaCrash()
    {
        CrashReport.testNativeCrash();
    }

    public void GetImei()
    {
        //实例化TelephonyManager对象
        TelephonyManager telephonyManager = (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);
//获取IMEI号
        try
        {
            if (telephonyManager.getPhoneCount() == 1) {
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                    UnityPlayer.UnitySendMessage("SDKManager", "GetImeiID", telephonyManager.getImei(0));
                    // 对于双卡设备，可以传入slotIndex
                }
            }
        } catch (Exception e) {
            Log.e("IMEI", "获取失败: " + e.getMessage());
        }
//获取IMEI1号
        try
        {
            if (telephonyManager.getPhoneCount() > 1) {
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                    UnityPlayer.UnitySendMessage("SDKManager", "GetImei2ID", telephonyManager.getImei(1));
                    // 参数1表示第二个SIM卡槽
                }
            }
        } catch (Exception e) {
            Log.e("IMEI", "获取失败: " + e.getMessage());
        }
        try
        {
            //获取MEID号
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                UnityPlayer.UnitySendMessage("SDKManager", "GetMeID", telephonyManager.getMeid());
                // 默认获取第一个SIM卡的MEID
            }
        } catch (Exception e) {
            Log.e("IMEI", "获取失败: " + e.getMessage());
        }
    }

    private boolean checkVersion() {
        if (Build.VERSION.SDK_INT >= 23)
            return true;
        return false;
    }

    private boolean checkTargetSdkVersion() {
        try {
            PackageInfo info = getPackageManager().getPackageInfo(getPackageName(), 0);
            int targetSdk = info.applicationInfo.targetSdkVersion;
            if (targetSdk >= 23)
                return true;
        } catch (android.content.pm.PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
        return false;
    }

    public void InitImei()
    {
        if (checkVersion()) {
            int result = -1;
            if (checkTargetSdkVersion()) {
                result = checkSelfPermission(Manifest.permission.READ_PHONE_STATE);
            } else {
                result = PermissionChecker.checkSelfPermission((Context) this, Manifest.permission.READ_PHONE_STATE);
            }
            if (result == 0)
            {
                GetImei();
                return;
            }
            requestPermissions(new String[] { Manifest.permission.READ_PHONE_STATE }, MY_PERMISSIONS_REQUEST_READ_PHONE_STATE);
        }
    }


    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == MY_PERMISSIONS_REQUEST_READ_PHONE_STATE) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                // 权限被授予，执行相关操作
                GetImei();
            } else {
                // 权限被拒绝，提示用户手动开启权限或功能受限提示
            }
        }
    }
}
