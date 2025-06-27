using DevStore.Pagamentos.AntiCorruption.Interfaces;

namespace DevStore.Pagamentos.AntiCorruption;

public class ConfigManager : IConfigManager
{
    public string GetValue(string node)
        => new([.. Enumerable.Repeat(node, 10).Select(s => s[new Random().Next(s.Length)])]);
}
