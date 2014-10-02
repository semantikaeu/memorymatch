namespace Semantika.Memory.Unity
{
    public interface IMyEuropeanaParser
    {
        T Parse<T>(string content) where T : class;
    }
}
