namespace FantasySky.CustomDF.Http.Client;

/// <summary>
/// 远程服务配置
/// </summary>
public class RemoteServiceConfiguration
{
    /// <summary>
    /// 基础地址
    /// </summary>
    public string BaseUrl { get; set; } = String.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = String.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = String.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = String.Empty;

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = String.Empty;
}
