using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Mustache;
using System.Text;

namespace host 
{
    public class ___test_Mustache
    { 

        public static void run2()
        {
            FormatCompiler compiler = new FormatCompiler();
            const string format = @"{{#set even}}
{{#each this}}
{{#if @even}}
Even
{{#else}}
Odd
{{/if}}
{{#set even}}
{{/each}}";

            Generator generator = compiler.Compile(format);
            generator.ValueRequested += (sender, e) =>
            {
                e.Value = !(bool)(e.Value ?? false);
            };
            string result = generator.Render(new int[] { 0, 1, 2, 3 });
        }

    }
}
