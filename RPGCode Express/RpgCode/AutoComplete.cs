/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012 Joshua Michael Daly
 * 
 * RPGCode Express is licensed under the GNU General Public License
 * version 3. See <http://www.gnu.org/licenses/> for more details.
 ********************************************************************
 */

using System;
using System.Collections;

namespace RpgCodeExpress.RpgCode
{
    /// <summary>
    /// 
    /// </summary>
    public class Autocomplete
    {
        public ArrayList IncludedClasses = new ArrayList();
        public ArrayList IncludedMethods = new ArrayList();
        public ArrayList IncludedVariables = new ArrayList();

        public ArrayList UserDefinedClasses = new ArrayList();
        public ArrayList UserDefinedMethods = new ArrayList();
        public ArrayList UserDefinedVariables = new ArrayList();

        public string[] Keywords = {"break", "return", "public", "private", "var", "null", 
                                       "include", "inline", "default", "on error", "resume next", 
                                       "goto", "redirect", "KillRedirect", "KillAllRedirects"};

        public string[] Constants = { "false", "this", "true" };

        public string[] StatementSnippets = {"if(^)" + Environment.NewLine + "{" + Environment.NewLine + ";" + 
                                                 Environment.NewLine + "}", "global(^)", "local(^)", 
                                     "if(^)" + Environment.NewLine + "{" + Environment.NewLine + ";" + 
                                     Environment.NewLine + "}" + Environment.NewLine + "else" + Environment.NewLine + 
                                     "{" + Environment.NewLine + ";" + Environment.NewLine + "}", 
                                     "for(^;;)" + Environment.NewLine + "{" + Environment.NewLine + ";" + 
                                     Environment.NewLine + "}", "while(^)" + Environment.NewLine + "{" + 
                                     Environment.NewLine + ";" + Environment.NewLine + "}", 
                                     "switch(^)" + Environment.NewLine + "{" + Environment.NewLine + "case" + 
                                     Environment.NewLine + "{" + Environment.NewLine + ";" + Environment.NewLine + "}" + 
                                     Environment.NewLine + "}", 
                                     "case^" + Environment.NewLine + "{" + Environment.NewLine + ";" + 
                                     Environment.NewLine + "}", "elseif(^)" + Environment.NewLine + "{" + 
                                     Environment.NewLine + ";" + Environment.NewLine + "}"};


        public string[] DeclartionSnippets = {"function ^" + Environment.NewLine + "{" + Environment.NewLine + "}", 
                                             "method ^" + Environment.NewLine + "{" + Environment.NewLine + "}", 
                                             "class ^" + Environment.NewLine + "{" + Environment.NewLine + "}", 
                                             "struct ^" + Environment.NewLine + "{" + Environment.NewLine + "}"};
    }
}
