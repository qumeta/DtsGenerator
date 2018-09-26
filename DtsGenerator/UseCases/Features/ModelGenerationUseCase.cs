using DtsGenerator.Converters;
using DtsGenerator.Emitters;
using DtsGenerator.Features.ModelAnalysis;
using DtsGenerator.Generators;
using DtsGenerator.Models;

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

            foreach (var tsModel in tsClassModels)
            {
                var contents = generator.GenerateClass(tsModel);

                emitter.Emit(_outputPath, tsModel.Name, EmittedFileType.Model, contents);

                //new TsGenerator().GenerateDataModelAST(tsModel, _outputPath);
            }

            foreach (var tsModel in tsEnumModels)
            {
                var contents = generator.GenerateEnum(tsModel);

                emitter.Emit(_outputPath, tsModel.Name, EmittedFileType.Enum, contents);

                //new TsGenerator().GenerateEnumAST(tsModel, _outputPath);
            }

            return Result.CreateSuccess();
        }
    }
}
