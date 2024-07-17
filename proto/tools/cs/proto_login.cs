// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: proto_login.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace Proto.basepb
{

    [global::ProtoBuf.ProtoContract()]
    public partial class LoginRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"account")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Account
        {
            get => __pbn__Account ?? "";
            set => __pbn__Account = value;
        }
        public bool ShouldSerializeAccount() => __pbn__Account != null;
        public void ResetAccount() => __pbn__Account = null;
        private string __pbn__Account;

        [global::ProtoBuf.ProtoMember(2, Name = @"passward")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Passward
        {
            get => __pbn__Passward ?? "";
            set => __pbn__Passward = value;
        }
        public bool ShouldSerializePassward() => __pbn__Passward != null;
        public void ResetPassward() => __pbn__Passward = null;
        private string __pbn__Passward;

        [global::ProtoBuf.ProtoMember(6, Name = @"deviceid")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Deviceid
        {
            get => __pbn__Deviceid ?? "";
            set => __pbn__Deviceid = value;
        }
        public bool ShouldSerializeDeviceid() => __pbn__Deviceid != null;
        public void ResetDeviceid() => __pbn__Deviceid = null;
        private string __pbn__Deviceid;

        [global::ProtoBuf.ProtoMember(7, Name = @"channel")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Channel
        {
            get => __pbn__Channel ?? "";
            set => __pbn__Channel = value;
        }
        public bool ShouldSerializeChannel() => __pbn__Channel != null;
        public void ResetChannel() => __pbn__Channel = null;
        private string __pbn__Channel;

        [global::ProtoBuf.ProtoMember(9, Name = @"imei")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Imei
        {
            get => __pbn__Imei ?? "";
            set => __pbn__Imei = value;
        }
        public bool ShouldSerializeImei() => __pbn__Imei != null;
        public void ResetImei() => __pbn__Imei = null;
        private string __pbn__Imei;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class LoginResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"account")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Account
        {
            get => __pbn__Account ?? "";
            set => __pbn__Account = value;
        }
        public bool ShouldSerializeAccount() => __pbn__Account != null;
        public void ResetAccount() => __pbn__Account = null;
        private string __pbn__Account;

        [global::ProtoBuf.ProtoMember(2, Name = @"token")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Token
        {
            get => __pbn__Token ?? "";
            set => __pbn__Token = value;
        }
        public bool ShouldSerializeToken() => __pbn__Token != null;
        public void ResetToken() => __pbn__Token = null;
        private string __pbn__Token;

        [global::ProtoBuf.ProtoMember(3, Name = @"name")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Name
        {
            get => __pbn__Name ?? "";
            set => __pbn__Name = value;
        }
        public bool ShouldSerializeName() => __pbn__Name != null;
        public void ResetName() => __pbn__Name = null;
        private string __pbn__Name;

        [global::ProtoBuf.ProtoMember(4)]
        [global::System.ComponentModel.DefaultValue("")]
        public string gameIP
        {
            get => __pbn__gameIP ?? "";
            set => __pbn__gameIP = value;
        }
        public bool ShouldSerializegameIP() => __pbn__gameIP != null;
        public void ResetgameIP() => __pbn__gameIP = null;
        private string __pbn__gameIP;

        [global::ProtoBuf.ProtoMember(9, Name = @"platform")]
        public int Platform
        {
            get => __pbn__Platform.GetValueOrDefault();
            set => __pbn__Platform = value;
        }
        public bool ShouldSerializePlatform() => __pbn__Platform != null;
        public void ResetPlatform() => __pbn__Platform = null;
        private int? __pbn__Platform;

        [global::ProtoBuf.ProtoMember(22)]
        [global::System.ComponentModel.DefaultValue("")]
        public string registerChannel
        {
            get => __pbn__registerChannel ?? "";
            set => __pbn__registerChannel = value;
        }
        public bool ShouldSerializeregisterChannel() => __pbn__registerChannel != null;
        public void ResetregisterChannel() => __pbn__registerChannel = null;
        private string __pbn__registerChannel;

        [global::ProtoBuf.ProtoMember(23)]
        public int loginType
        {
            get => __pbn__loginType.GetValueOrDefault();
            set => __pbn__loginType = value;
        }
        public bool ShouldSerializeloginType() => __pbn__loginType != null;
        public void ResetloginType() => __pbn__loginType = null;
        private int? __pbn__loginType;

    }

    [global::ProtoBuf.ProtoContract(Name = @"Request_PhoneCode")]
    public partial class RequestPhoneCode : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string phoneNumber
        {
            get => __pbn__phoneNumber ?? "";
            set => __pbn__phoneNumber = value;
        }
        public bool ShouldSerializephoneNumber() => __pbn__phoneNumber != null;
        public void ResetphoneNumber() => __pbn__phoneNumber = null;
        private string __pbn__phoneNumber;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SuperSdkInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string sdkTicket
        {
            get => __pbn__sdkTicket ?? "";
            set => __pbn__sdkTicket = value;
        }
        public bool ShouldSerializesdkTicket() => __pbn__sdkTicket != null;
        public void ResetsdkTicket() => __pbn__sdkTicket = null;
        private string __pbn__sdkTicket;

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string opId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string opGameId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClientInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public string pubKey { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string deviceId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string clientVersion { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public long worldId { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public long roleId { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        [global::System.ComponentModel.DefaultValue("")]
        public string operationSystem
        {
            get => __pbn__operationSystem ?? "";
            set => __pbn__operationSystem = value;
        }
        public bool ShouldSerializeoperationSystem() => __pbn__operationSystem != null;
        public void ResetoperationSystem() => __pbn__operationSystem = null;
        private string __pbn__operationSystem;

        [global::ProtoBuf.ProtoMember(7, Name = @"platform")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Platform
        {
            get => __pbn__Platform ?? "";
            set => __pbn__Platform = value;
        }
        public bool ShouldSerializePlatform() => __pbn__Platform != null;
        public void ResetPlatform() => __pbn__Platform = null;
        private string __pbn__Platform;

        [global::ProtoBuf.ProtoMember(8)]
        [global::System.ComponentModel.DefaultValue("")]
        public string resVersion
        {
            get => __pbn__resVersion ?? "";
            set => __pbn__resVersion = value;
        }
        public bool ShouldSerializeresVersion() => __pbn__resVersion != null;
        public void ResetresVersion() => __pbn__resVersion = null;
        private string __pbn__resVersion;

        [global::ProtoBuf.ProtoMember(9, Name = @"country")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Country
        {
            get => __pbn__Country ?? "";
            set => __pbn__Country = value;
        }
        public bool ShouldSerializeCountry() => __pbn__Country != null;
        public void ResetCountry() => __pbn__Country = null;
        private string __pbn__Country;

        [global::ProtoBuf.ProtoMember(10, Name = @"model")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Model
        {
            get => __pbn__Model ?? "";
            set => __pbn__Model = value;
        }
        public bool ShouldSerializeModel() => __pbn__Model != null;
        public void ResetModel() => __pbn__Model = null;
        private string __pbn__Model;

        [global::ProtoBuf.ProtoMember(11)]
        [global::System.ComponentModel.DefaultValue("")]
        public string bundleId
        {
            get => __pbn__bundleId ?? "";
            set => __pbn__bundleId = value;
        }
        public bool ShouldSerializebundleId() => __pbn__bundleId != null;
        public void ResetbundleId() => __pbn__bundleId = null;
        private string __pbn__bundleId;

        [global::ProtoBuf.ProtoMember(12)]
        public int languageId
        {
            get => __pbn__languageId.GetValueOrDefault();
            set => __pbn__languageId = value;
        }
        public bool ShouldSerializelanguageId() => __pbn__languageId != null;
        public void ResetlanguageId() => __pbn__languageId = null;
        private int? __pbn__languageId;

        [global::ProtoBuf.ProtoMember(13)]
        [global::System.ComponentModel.DefaultValue("")]
        public string clientTimeZone
        {
            get => __pbn__clientTimeZone ?? "";
            set => __pbn__clientTimeZone = value;
        }
        public bool ShouldSerializeclientTimeZone() => __pbn__clientTimeZone != null;
        public void ResetclientTimeZone() => __pbn__clientTimeZone = null;
        private string __pbn__clientTimeZone;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ReconnectRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public global::com.bochsler.protocol.LoginType loginType { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public SuperSdkInfo sdkInfo { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public ClientInfo clientInfo { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public int reconnectType { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public int msgIndex { get; set; }

        [global::ProtoBuf.ProtoMember(6, IsRequired = true)]
        public ReconnectParam reconnectParam { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ReconnectParam : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string clientIp
        {
            get => __pbn__clientIp ?? "";
            set => __pbn__clientIp = value;
        }
        public bool ShouldSerializeclientIp() => __pbn__clientIp != null;
        public void ResetclientIp() => __pbn__clientIp = null;
        private string __pbn__clientIp;

        [global::ProtoBuf.ProtoMember(2, Name = @"time", IsRequired = true)]
        public long Time { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ReconnectResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"result", IsRequired = true)]
        public LoginResult Result { get; set; } = LoginResult.LoginSuccess;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string playerId
        {
            get => __pbn__playerId ?? "";
            set => __pbn__playerId = value;
        }
        public bool ShouldSerializeplayerId() => __pbn__playerId != null;
        public void ResetplayerId() => __pbn__playerId = null;
        private string __pbn__playerId;

        [global::ProtoBuf.ProtoMember(3)]
        public int timeZone
        {
            get => __pbn__timeZone.GetValueOrDefault();
            set => __pbn__timeZone = value;
        }
        public bool ShouldSerializetimeZone() => __pbn__timeZone != null;
        public void ResettimeZone() => __pbn__timeZone = null;
        private int? __pbn__timeZone;

        [global::ProtoBuf.ProtoMember(4, Name = @"servertime")]
        public long Servertime
        {
            get => __pbn__Servertime.GetValueOrDefault();
            set => __pbn__Servertime = value;
        }
        public bool ShouldSerializeServertime() => __pbn__Servertime != null;
        public void ResetServertime() => __pbn__Servertime = null;
        private long? __pbn__Servertime;

        [global::ProtoBuf.ProtoMember(5, Name = @"serverkey")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Serverkey
        {
            get => __pbn__Serverkey ?? "";
            set => __pbn__Serverkey = value;
        }
        public bool ShouldSerializeServerkey() => __pbn__Serverkey != null;
        public void ResetServerkey() => __pbn__Serverkey = null;
        private string __pbn__Serverkey;

        [global::ProtoBuf.ProtoMember(6)]
        public PlayerInfo playerInfo { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public long allianceId
        {
            get => __pbn__allianceId.GetValueOrDefault();
            set => __pbn__allianceId = value;
        }
        public bool ShouldSerializeallianceId() => __pbn__allianceId != null;
        public void ResetallianceId() => __pbn__allianceId = null;
        private long? __pbn__allianceId;

        [global::ProtoBuf.ProtoMember(8)]
        public long allianceWorldId
        {
            get => __pbn__allianceWorldId.GetValueOrDefault();
            set => __pbn__allianceWorldId = value;
        }
        public bool ShouldSerializeallianceWorldId() => __pbn__allianceWorldId != null;
        public void ResetallianceWorldId() => __pbn__allianceWorldId = null;
        private long? __pbn__allianceWorldId;

        [global::ProtoBuf.ProtoMember(9)]
        public int msgIndex
        {
            get => __pbn__msgIndex.GetValueOrDefault();
            set => __pbn__msgIndex = value;
        }
        public bool ShouldSerializemsgIndex() => __pbn__msgIndex != null;
        public void ResetmsgIndex() => __pbn__msgIndex = null;
        private int? __pbn__msgIndex;

        [global::ProtoBuf.ProtoMember(10)]
        [global::System.ComponentModel.DefaultValue("")]
        public string opGameId
        {
            get => __pbn__opGameId ?? "";
            set => __pbn__opGameId = value;
        }
        public bool ShouldSerializeopGameId() => __pbn__opGameId != null;
        public void ResetopGameId() => __pbn__opGameId = null;
        private string __pbn__opGameId;

        [global::ProtoBuf.ProtoMember(11)]
        [global::System.ComponentModel.DefaultValue("")]
        public string opId
        {
            get => __pbn__opId ?? "";
            set => __pbn__opId = value;
        }
        public bool ShouldSerializeopId() => __pbn__opId != null;
        public void ResetopId() => __pbn__opId = null;
        private string __pbn__opId;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class PlayerInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"csid")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Csid
        {
            get => __pbn__Csid ?? "";
            set => __pbn__Csid = value;
        }
        public bool ShouldSerializeCsid() => __pbn__Csid != null;
        public void ResetCsid() => __pbn__Csid = null;
        private string __pbn__Csid;

        [global::ProtoBuf.ProtoMember(2, Name = @"nickname")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Nickname
        {
            get => __pbn__Nickname ?? "";
            set => __pbn__Nickname = value;
        }
        public bool ShouldSerializeNickname() => __pbn__Nickname != null;
        public void ResetNickname() => __pbn__Nickname = null;
        private string __pbn__Nickname;

        [global::ProtoBuf.ProtoMember(3, Name = @"icon")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Icon
        {
            get => __pbn__Icon ?? "";
            set => __pbn__Icon = value;
        }
        public bool ShouldSerializeIcon() => __pbn__Icon != null;
        public void ResetIcon() => __pbn__Icon = null;
        private string __pbn__Icon;

        [global::ProtoBuf.ProtoMember(4, Name = @"sex")]
        public int Sex
        {
            get => __pbn__Sex.GetValueOrDefault();
            set => __pbn__Sex = value;
        }
        public bool ShouldSerializeSex() => __pbn__Sex != null;
        public void ResetSex() => __pbn__Sex = null;
        private int? __pbn__Sex;

        [global::ProtoBuf.ProtoMember(5, Name = @"level")]
        public int Level
        {
            get => __pbn__Level.GetValueOrDefault();
            set => __pbn__Level = value;
        }
        public bool ShouldSerializeLevel() => __pbn__Level != null;
        public void ResetLevel() => __pbn__Level = null;
        private int? __pbn__Level;

        [global::ProtoBuf.ProtoMember(6, Name = @"exp")]
        public int Exp
        {
            get => __pbn__Exp.GetValueOrDefault();
            set => __pbn__Exp = value;
        }
        public bool ShouldSerializeExp() => __pbn__Exp != null;
        public void ResetExp() => __pbn__Exp = null;
        private int? __pbn__Exp;

        [global::ProtoBuf.ProtoMember(7)]
        public bool isVipActive
        {
            get => __pbn__isVipActive.GetValueOrDefault();
            set => __pbn__isVipActive = value;
        }
        public bool ShouldSerializeisVipActive() => __pbn__isVipActive != null;
        public void ResetisVipActive() => __pbn__isVipActive = null;
        private bool? __pbn__isVipActive;

        [global::ProtoBuf.ProtoMember(8)]
        public int vipLevel
        {
            get => __pbn__vipLevel.GetValueOrDefault();
            set => __pbn__vipLevel = value;
        }
        public bool ShouldSerializevipLevel() => __pbn__vipLevel != null;
        public void ResetvipLevel() => __pbn__vipLevel = null;
        private int? __pbn__vipLevel;

        [global::ProtoBuf.ProtoMember(9)]
        public int vipExp
        {
            get => __pbn__vipExp.GetValueOrDefault();
            set => __pbn__vipExp = value;
        }
        public bool ShouldSerializevipExp() => __pbn__vipExp != null;
        public void ResetvipExp() => __pbn__vipExp = null;
        private int? __pbn__vipExp;

        [global::ProtoBuf.ProtoMember(10, IsRequired = true)]
        public string playerId { get; set; }

        [global::ProtoBuf.ProtoMember(11)]
        [global::System.ComponentModel.DefaultValue("")]
        public string allianceId
        {
            get => __pbn__allianceId ?? "";
            set => __pbn__allianceId = value;
        }
        public bool ShouldSerializeallianceId() => __pbn__allianceId != null;
        public void ResetallianceId() => __pbn__allianceId = null;
        private string __pbn__allianceId;

        [global::ProtoBuf.ProtoMember(12, Name = @"power")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Power
        {
            get => __pbn__Power ?? "";
            set => __pbn__Power = value;
        }
        public bool ShouldSerializePower() => __pbn__Power != null;
        public void ResetPower() => __pbn__Power = null;
        private string __pbn__Power;

        [global::ProtoBuf.ProtoMember(13)]
        public long createTime
        {
            get => __pbn__createTime.GetValueOrDefault();
            set => __pbn__createTime = value;
        }
        public bool ShouldSerializecreateTime() => __pbn__createTime != null;
        public void ResetcreateTime() => __pbn__createTime = null;
        private long? __pbn__createTime;

        [global::ProtoBuf.ProtoMember(14)]
        [global::System.ComponentModel.DefaultValue("")]
        public string onlineAddictionTime
        {
            get => __pbn__onlineAddictionTime ?? "";
            set => __pbn__onlineAddictionTime = value;
        }
        public bool ShouldSerializeonlineAddictionTime() => __pbn__onlineAddictionTime != null;
        public void ResetonlineAddictionTime() => __pbn__onlineAddictionTime = null;
        private string __pbn__onlineAddictionTime;

        [global::ProtoBuf.ProtoMember(15)]
        public int antiAddicationFlag
        {
            get => __pbn__antiAddicationFlag.GetValueOrDefault();
            set => __pbn__antiAddicationFlag = value;
        }
        public bool ShouldSerializeantiAddicationFlag() => __pbn__antiAddicationFlag != null;
        public void ResetantiAddicationFlag() => __pbn__antiAddicationFlag = null;
        private int? __pbn__antiAddicationFlag;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class LoginExtraData : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"banshu")]
        public bool Banshu
        {
            get => __pbn__Banshu.GetValueOrDefault();
            set => __pbn__Banshu = value;
        }
        public bool ShouldSerializeBanshu() => __pbn__Banshu != null;
        public void ResetBanshu() => __pbn__Banshu = null;
        private bool? __pbn__Banshu;

        [global::ProtoBuf.ProtoMember(2, Name = @"account")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Account
        {
            get => __pbn__Account ?? "";
            set => __pbn__Account = value;
        }
        public bool ShouldSerializeAccount() => __pbn__Account != null;
        public void ResetAccount() => __pbn__Account = null;
        private string __pbn__Account;

        [global::ProtoBuf.ProtoMember(3, Name = @"country")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Country
        {
            get => __pbn__Country ?? "";
            set => __pbn__Country = value;
        }
        public bool ShouldSerializeCountry() => __pbn__Country != null;
        public void ResetCountry() => __pbn__Country = null;
        private string __pbn__Country;

        [global::ProtoBuf.ProtoMember(4, Name = @"lang")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Lang
        {
            get => __pbn__Lang ?? "";
            set => __pbn__Lang = value;
        }
        public bool ShouldSerializeLang() => __pbn__Lang != null;
        public void ResetLang() => __pbn__Lang = null;
        private string __pbn__Lang;

        [global::ProtoBuf.ProtoMember(5)]
        [global::System.ComponentModel.DefaultValue("")]
        public string apiUrl
        {
            get => __pbn__apiUrl ?? "";
            set => __pbn__apiUrl = value;
        }
        public bool ShouldSerializeapiUrl() => __pbn__apiUrl != null;
        public void ResetapiUrl() => __pbn__apiUrl = null;
        private string __pbn__apiUrl;

        [global::ProtoBuf.ProtoMember(6)]
        [global::System.ComponentModel.DefaultValue("")]
        public string cdnUrl
        {
            get => __pbn__cdnUrl ?? "";
            set => __pbn__cdnUrl = value;
        }
        public bool ShouldSerializecdnUrl() => __pbn__cdnUrl != null;
        public void ResetcdnUrl() => __pbn__cdnUrl = null;
        private string __pbn__cdnUrl;

        [global::ProtoBuf.ProtoMember(7)]
        [global::System.ComponentModel.DefaultValue("")]
        public string battleRecordPath
        {
            get => __pbn__battleRecordPath ?? "";
            set => __pbn__battleRecordPath = value;
        }
        public bool ShouldSerializebattleRecordPath() => __pbn__battleRecordPath != null;
        public void ResetbattleRecordPath() => __pbn__battleRecordPath = null;
        private string __pbn__battleRecordPath;

        [global::ProtoBuf.ProtoMember(8)]
        [global::System.ComponentModel.DefaultValue("")]
        public string clientIp
        {
            get => __pbn__clientIp ?? "";
            set => __pbn__clientIp = value;
        }
        public bool ShouldSerializeclientIp() => __pbn__clientIp != null;
        public void ResetclientIp() => __pbn__clientIp = null;
        private string __pbn__clientIp;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class LoginSuccReq : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class LoginSuccResp : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ReconnectMsgIndex : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public int msgType { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public int sendSN { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int recvSN { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public int pushSN { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public int broadcastSN { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClientVersion : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public string opGameId { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string opId { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string clientVersion { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public string extraVersion { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public string resLimitVersion { get; set; }

        [global::ProtoBuf.ProtoMember(6, IsRequired = true)]
        public string resLatestVersion { get; set; }

        [global::ProtoBuf.ProtoMember(7, IsRequired = true)]
        public string forceUpdateUrl { get; set; }

        [global::ProtoBuf.ProtoMember(8, Name = @"cdnArray")]
        public global::System.Collections.Generic.List<string> cdnArrays { get; } = new global::System.Collections.Generic.List<string>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SCClientVersionList : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"version", IsRequired = true)]
        public ClientVersion Version { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClientVersionPreUpdateRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"platform", IsRequired = true)]
        public string Platform { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string clientChannel { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClientVersionPreUpdateResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ClientVersion clientVersion { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public enum LoginResult
    {
        [global::ProtoBuf.ProtoEnum(Name = @"LOGIN_SUCCESS")]
        LoginSuccess = 1,
        [global::ProtoBuf.ProtoEnum(Name = @"VERSION_ERROR")]
        VersionError = 2,
        [global::ProtoBuf.ProtoEnum(Name = @"NOT_IN_WHITELIST")]
        NotInWhitelist = 3,
        [global::ProtoBuf.ProtoEnum(Name = @"SDK_TIMESTAMP_ERROR")]
        SdkTimestampError = 4,
        [global::ProtoBuf.ProtoEnum(Name = @"SDK_SIGN_ERROR")]
        SdkSignError = 5,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_IP_ERROR")]
        ParamIpError = 6,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_ACCOUNT_ERROR")]
        ParamAccountError = 7,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_ROLE_ERROR")]
        ParamRoleError = 8,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_WORLD_ERROR")]
        ParamWorldError = 9,
        [global::ProtoBuf.ProtoEnum(Name = @"ROLE_BE_BANNED")]
        RoleBeBanned = 10,
        [global::ProtoBuf.ProtoEnum(Name = @"ROLE_COUNT_MAX")]
        RoleCountMax = 11,
        [global::ProtoBuf.ProtoEnum(Name = @"ROLE_CREATE_ERROR")]
        RoleCreateError = 12,
        [global::ProtoBuf.ProtoEnum(Name = @"ACCOUNT_IS_LOGGING")]
        AccountIsLogging = 13,
        [global::ProtoBuf.ProtoEnum(Name = @"INTERNAL_ERROR")]
        InternalError = 14,
        [global::ProtoBuf.ProtoEnum(Name = @"LOGIN_SIGN_ERROR")]
        LoginSignError = 15,
        [global::ProtoBuf.ProtoEnum(Name = @"RECONNECT_ERROR")]
        ReconnectError = 16,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_DEVICE_ERROR")]
        ParamDeviceError = 17,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_TIMESTAMP_ERROR")]
        ParamTimestampError = 18,
        [global::ProtoBuf.ProtoEnum(Name = @"PARAM_NOTNULL_ERROR")]
        ParamNotnullError = 19,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
