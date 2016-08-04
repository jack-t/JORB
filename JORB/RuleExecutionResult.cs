namespace JORB
{
    public class RuleExecutionResult
    {
        public RuleExecutionException Exception { get; }
        public string RuleName { get; }

        private readonly bool? _result;
        public bool Result
        {
            get
            {
                if (_result.HasValue) return _result.Value;
                throw new RuleExecutionException("Caused by inner exception: ", Exception);
            }
        }

        public bool HasResult => _result.HasValue;

        public RuleExecutionResult(string name, bool? result, RuleExecutionException ex = null)
        {
            RuleName = name;
            _result = result;
            Exception = ex;
        }

    }
}