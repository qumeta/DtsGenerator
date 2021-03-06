﻿using DtsGenerator.Converters;
using DtsGenerator.Emitters;
using DtsGenerator.Features.ModelAnalysis;
using DtsGenerator.Generators;
using DtsGenerator.Models;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.UseCases.Features
{
    public class ModelGenerationUseCase
    {
        private string _inputPath;
        private string _outputPath;

        public string Description => "Generate TypeScript model classes from C# DTO objects.";

        public ModelGenerationUseCase(string inputPath, string outputPath)
        {
            _inputPath = inputPath;
            _outputPath = outputPath;
        }

        public Result Handle()
        {
            var analyzer = new ModelAnalyzer(new ModelAnalysisContext());
            var converter = new ModelConverter();
            var generator = new ModelGenerator();
            var emitter = new TypeScriptEmitter();

            var analysisResult = analyzer.Analyze(_inputPath);

            if (!analysisResult.Success)
            {
                return Result.CreateError($"Source analysis error: {analysisResult.ErrorMessage}");
            }

            var tsClassModels = converter.ConvertClasses(analysisResult.Value.Classes);
            var tsEnumModels = converter.ConvertEnums(analysisResult.Value.Enums);

            // enum
            var enumAllContents = new StringBuilder();
            enumAllContents.AppendLine("// ---------------------------");
            enumAllContents.AppendLine("// auto-generated by DtsGenerator");
            enumAllContents.AppendLine("// ---------------------------");
            // declare namespace server {
            enumAllContents.AppendLine("declare namespace server {");
            enumAllContents.AppendLine();

            var enumNameList = new List<string>();

            foreach (var tsModel in tsEnumModels)
            {
                var contents = generator.GenerateEnum(tsModel);

                //emitter.Emit(_outputPath, tsModel.Name, EmittedFileType.Enum, contents);

                //new TsGenerator().GenerateEnumAST(tsModel, _outputPath);
                enumAllContents.AppendLine(generator.GenerateEnumData(tsModel));

                enumNameList.Add(tsModel.Name);
            }
            enumAllContents.AppendLine("}");
            emitter.EmitAll(_outputPath, EmittedFileType.Enum, enumAllContents.ToString());

            // class model
            var classAllContents = new StringBuilder();
            classAllContents.AppendLine("// ---------------------------");
            classAllContents.AppendLine("// auto-generated by DtsGenerator");
            classAllContents.AppendLine("// ---------------------------");
            classAllContents.AppendLine();


            // import { EnumProtocol } from './enums';
            //classAllContents.AppendLine($"import {{ {string.Join(", ", enumNameList)} }} from './enums';");
            //classAllContents.AppendLine($"export {{ {string.Join(", ", enumNameList)} }} from './enums';");
            // declare namespace server {
            classAllContents.AppendLine("declare namespace server {");
            classAllContents.AppendLine();

            foreach (var tsModel in tsClassModels)
            {
                var contents = generator.GenerateClass(tsModel);

                //emitter.Emit(_outputPath, tsModel.Name, EmittedFileType.Model, contents);

                //new TsGenerator().GenerateDataModelAST(tsModel, _outputPath);
                classAllContents.AppendLine(generator.GenerateClassData(tsModel));
            }
            classAllContents.AppendLine("}");

            emitter.EmitAll(_outputPath, EmittedFileType.Model, classAllContents.ToString());

            return Result.CreateSuccess();
        }
    }
}
