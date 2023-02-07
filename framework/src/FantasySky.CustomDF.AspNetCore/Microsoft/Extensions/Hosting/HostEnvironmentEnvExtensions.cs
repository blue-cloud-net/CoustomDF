namespace Microsoft.Extensions.Hosting;

//
// 摘要:
//     Extension methods for Microsoft.Extensions.Hosting.IHostEnvironment.
public static class HostEnvironmentEnvExtensions
{
    //
    // 摘要:
    //     Checks if the current host environment name is Microsoft.Extensions.Hosting.EnvironmentName.Development.
    //
    // 参数:
    //   hostEnvironment:
    //     An instance of Microsoft.Extensions.Hosting.IHostEnvironment.
    //
    // 返回结果:
    //     True if the environment name is Microsoft.Extensions.Hosting.EnvironmentName.Development,
    //     otherwise false.
    public static bool IsTest(this IHostEnvironment hostEnvironment)
        => hostEnvironment.IsEnvironment("Test");

}
