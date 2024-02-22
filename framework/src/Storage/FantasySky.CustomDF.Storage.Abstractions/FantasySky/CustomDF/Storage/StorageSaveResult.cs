namespace FantasySky.CustomDF.Storage;

/// <summary>
/// 存储保存结果
/// </summary>
public enum StorageSaveResult
{
    /// <summary>
    /// 该路径存储冲突
    /// </summary>
    Conflict,

    /// <summary>
    /// 该路径文件已存在
    /// </summary>
    AlreadyExists,

    /// <summary>
    /// 保存成功
    /// </summary>
    Success,
}