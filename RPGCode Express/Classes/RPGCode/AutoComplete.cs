/*
 ********************************************************************
 * RPGCode Express Version 1
 * This file copyright (C) 2012  Joshua Michael Daly
 ********************************************************************
 * This file is part of RPGCode Express Version 1.
 *
 * RPGCode Express is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * RPGCode Express is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with RPGCode Express.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using FastColoredTextBoxNS;

namespace RPGCode_Express.Classes.RPGCode
{
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

        public string[] Constants = { "false", "true" };

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
