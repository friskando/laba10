using System.Collections.Generic;

namespace ConsoleApp1
{
    class Keywords
    {
        private Dictionary<string, byte> _keywords;

        public Keywords()
        {
            _keywords = new Dictionary<string, byte>();
            _keywords["do"] = LexicalAnalyzer.dosy;
            _keywords["if"] = LexicalAnalyzer.ifsy;
            _keywords["in"] = LexicalAnalyzer.insy;
            _keywords["of"] = LexicalAnalyzer.ofsy;
            _keywords["or"] = LexicalAnalyzer.orsy;
            _keywords["to"] = LexicalAnalyzer.tosy;
            _keywords["end"] = LexicalAnalyzer.endsy;
            _keywords["var"] = LexicalAnalyzer.varsy;
            _keywords["div"] = LexicalAnalyzer.divsy;
            _keywords["and"] = LexicalAnalyzer.andsy;
            _keywords["not"] = LexicalAnalyzer.notsy;
            _keywords["for"] = LexicalAnalyzer.forsym;
            _keywords["mod"] = LexicalAnalyzer.modsy;
            _keywords["nil"] = LexicalAnalyzer.nilsy;
            _keywords["set"] = LexicalAnalyzer.setsy;
            _keywords["then"] = LexicalAnalyzer.thensy;
            _keywords["else"] = LexicalAnalyzer.elsesy;
            _keywords["case"] = LexicalAnalyzer.casesy;
            _keywords["file"] = LexicalAnalyzer.filesy;
            _keywords["goto"] = LexicalAnalyzer.gotosy;
            _keywords["type"] = LexicalAnalyzer.typesy;
            _keywords["with"] = LexicalAnalyzer.withsy;
            _keywords["begin"] = LexicalAnalyzer.beginsy;
            _keywords["while"] = LexicalAnalyzer.whilesy;
            _keywords["array"] = LexicalAnalyzer.arraysy;
            _keywords["const"] = LexicalAnalyzer.constsy;
            _keywords["label"] = LexicalAnalyzer.labelsy;
            _keywords["until"] = LexicalAnalyzer.untilsy;
            _keywords["downto"] = LexicalAnalyzer.downtosy;
            _keywords["packed"] = LexicalAnalyzer.packedsy;
            _keywords["record"] = LexicalAnalyzer.recordsy;
            _keywords["repeat"] = LexicalAnalyzer.repeatsy;
            _keywords["program"] = LexicalAnalyzer.programsy;
            _keywords["function"] = LexicalAnalyzer.functionsy;
            _keywords["procedure"] = LexicalAnalyzer.procedurensy;
        }

        public byte CheckKeyword(string name)
        {
            name = name.ToLower();
            if (_keywords.ContainsKey(name))
                return _keywords[name];
            return 0;
        }
    }
}