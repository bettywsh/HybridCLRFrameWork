package com.mtgame.com.oaId;

import android.app.Application;

public class OAIDApplication extends Application {
    public static final String TAG = "OAIDApplication";
    @Override
    public void onCreate() {
        super.onCreate();
        System.loadLibrary("msaoaidsec");  // TODO （3）SDK初始化操作
    }

}
