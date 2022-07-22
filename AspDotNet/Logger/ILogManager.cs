namespace HermesCenter.Logger
{
    /*
        This service is a logger - not really clear its functionality at the moment - need to specify to understand.

        Note: In primitive project, this service is separated into 'Project' (Children of the 'Solution'). So we might need to split it 
        into another place so far.
     */
    public interface ILogManager
    {
        void Error(Exception ex, string message, string prefix = "");
        void Error(string message, string prefix = "");
        void Error(Exception ex, string template, params object[] protertyValues);
        void Information(string message, string prefix = "");
        void Information(string template, params object[] protertyValues);
        void Warning(string template, params object [] protertyValues);
    }
}
