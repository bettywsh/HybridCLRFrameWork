public enum EUILayer : int
{
    Normal = 0,
    
    Text = 110, //Ʈ��
    Dialog = 111, //��ʾ����
    Guide = 122, //��������
    Loading = 133, //�л�����
    DialogSystem = 151, //ϵͳ����
}


public enum EScene : int
{ 
    Login = 1,
    Main,
    Battle,
}

public enum EAttribute : int
{
    Scene = 1,
    Panel,
    SubPanel,
    Cell
}
