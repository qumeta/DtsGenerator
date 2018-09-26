/*
 * 分析c#model文件生成对应的ts接口文件
 *
*/

using System;
using DtsGenerator.UseCases.Features;

namespace DtsGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // "Game.Models"
            //var inputPath = @"C:\Item\youigame\trunk\server\game\Game.Models\Game.Models.csproj";
            //var inputPath = @"C:\Item\youigame\trunk\tools\DtsGenerator\Game.Models\Game.Models.csproj";
            //var inputPath = @"C:\Item\youigame\trunk\tools\DtsGenerator\DtsGenerator.sln";
            //var outputPath = @"C:\Item\youigame\trunk\tools\DtsGenerator\bond\ts";
            if(args.Length < 2)
            {
                Console.WriteLine("DtsGenerator inputPath outputPath");
                return;
            }

            var inputPath = args[0];
            var outputPath = args[1];
            Run(inputPath, outputPath);
        }

        private static void Run(string inputPath, string outputPath)
        {
            try
            {
                var useCase = new ModelGenerationUseCase(inputPath, outputPath);

                var result = useCase.Handle();

                if (!result.Success)
                {
                    Console.WriteLine(result.ErrorString());
                }
            }
            catch
            {

            }
        }

        /*
        static async Task Test()
        {
            var file = @"C:\Item\youigame\trunk\tools\DtsGenerator\DtsGenerator\data_types.cs";
            var tree = SyntaxFactory.ParseSyntaxTree(System.IO.File.ReadAllText(file));
            var unit_syntax = tree.GetCompilationUnitRoot();

            var ns = (NamespaceDeclarationSyntax)unit_syntax.Members.Where(item => item.IsKind(SyntaxKind.NamespaceDeclaration)).FirstOrDefault();
            var css = ns.Members.Where(item => item.IsKind(SyntaxKind.ClassDeclaration));

            foreach(ClassDeclarationSyntax cs in css)
            {
                Console.WriteLine(cs.Identifier.Text);

                var pss = cs.Members.Where(item => item.IsKind(SyntaxKind.PropertyDeclaration));

                foreach(PropertyDeclarationSyntax ps in pss)
                {
                    Console.WriteLine(ps.Identifier.Text);
                }
            }

            Console.WriteLine();
        }
        */
    }
}
