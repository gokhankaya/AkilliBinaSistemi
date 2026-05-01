namespace AdleGraph.Interface
{
    public interface IRScriptRunner
    {
        string RunFromCmd(string rCodeFilePath, string rScriptExecutablePath = "rscript.exe", string args = "");
    }
}