using System.Collections.Generic;

namespace KieServerAdapter
{
    public class ExecutionResult
    {
        public List<KeyValuePair<string, object>> Results;
        public List<KeyValuePair<string, object>> Facts;
    }
}
