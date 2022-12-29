using System.Collections.Generic;

namespace OT.Extensions.Types
{
    public class AssemblyDefinition
    {
        public string name;
        public string rootNamespace;

        public List<string> references = new List<string>();

        public List<string> includePlatforms = new List<string>();

        public List<string> excludePlatforms = new List<string>();

        public List<string> optionalUnityReferences = new List<string>();

    }
}