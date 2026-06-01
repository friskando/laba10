using System.Collections.Generic;

namespace ConsoleApp1
{
    class Keywords
    {
        private Dictionary<string, byte> keywords;

        public Keywords()
        {
            keywords = new Dictionary<string, byte>();

            keywords["do"] = LexicalAnalyzer.dosy;
            keywords["if"] = LexicalAnalyzer.ifsy;
            keywords["in"] = LexicalAnalyzer.insy;
            keywords["of"] = LexicalAnalyzer.ofsy;
            keywords["or"] = LexicalAnalyzer.orsy;
            keywords["to"] = LexicalAnalyzer.tosy;
            keywords["end"] = LexicalAnalyzer.endsy;
            keywords["var"] = LexicalAnalyzer.varsy;
            keywords["div"] = LexicalAnalyzer.divsy;
            keywords["and"] = LexicalAnalyzer.andsy;
            keywords["not"] = LexicalAnalyzer.notsy;
            keywords["for"] = LexicalAnalyzer.forsym;
            keywords["mod"] = LexicalAnalyzer.modsy;
            keywords["nil"] = LexicalAnalyzer.nilsy;
            keywords["set"] = LexicalAnalyzer.setsy;
            keywords["then"] = LexicalAnalyzer.thensy;
            keywords["else"] = LexicalAnalyzer.elsesy;
            keywords["case"] = LexicalAnalyzer.casesy;
            keywords["file"] = LexicalAnalyzer.filesy;
            keywords["goto"] = LexicalAnalyzer.gotosy;
            keywords["type"] = LexicalAnalyzer.typesy;
            keywords["with"] = LexicalAnalyzer.withsy;
            keywords["begin"] = LexicalAnalyzer.beginsy;
            keywords["while"] = LexicalAnalyzer.whilesy;
            keywords["array"] = LexicalAnalyzer.arraysy;
            keywords["const"] = LexicalAnalyzer.constsy;
            keywords["label"] = LexicalAnalyzer.labelsy;
            keywords["until"] = LexicalAnalyzer.untilsy;
            keywords["downto"] = LexicalAnalyzer.downtosy;
            keywords["packed"] = LexicalAnalyzer.packedsy;
            keywords["record"] = LexicalAnalyzer.recordsy;
            keywords["repeat"] = LexicalAnalyzer.repeatsy;
            keywords["program"] = LexicalAnalyzer.programsy;
            keywords["function"] = LexicalAnalyzer.functionsy;
            keywords["procedure"] = LexicalAnalyzer.procedurensy;
        }

        public byte CheckKeyword(string name)
        {
            name = name.ToLower();
            if (keywords.ContainsKey(name))
            { 
                return keywords[name];
            }
            return 0;
        }
    }
}