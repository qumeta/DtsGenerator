using DtsGenerator.Constants;
using DtsGenerator.CSharp;
using DtsGenerator.TypeScript;
using DtsGenerator.TypeScriptDTO;
using DtsGenerator.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DtsGenerator.Generators
{
    public class TsGenerator
    {
        //private readonly HttpClient _client;

        public TsGenerator()
        {
            //_client = new HttpClient();

            //_client.BaseAddress = new Uri("http://localhost:8080");
            //_client.DefaultRequestHeaders.Accept.Clear();
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string CallGenerator(string path, HttpContent content)
        {
            //var response = _client.PostAsync(path, content).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    return response.Content.ReadAsStringAsync().Result;
            //}

            return string.Empty;
        }

        private StringContent CreateStringContent(object value)
        {
            return new StringContent(
                JsonConvert.SerializeObject(
                    value,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }).ToString(),
                Encoding.UTF8,
                "application/json"
            );
        }

        public void GenerateDataModelAST(TypeScriptClassModel classModel, string outputPath)
        {
            var request = new ClassGenerationRequest();

            string fileName = $"{NameCaseConverter.ToKebabCase(classModel.Name)}.model.{TypeScriptFileExtension.File}";

            var typeGenerator = new TypeGenerator();

            request.OutputPath = Path.Combine(outputPath, "models", fileName);

            request.DataModel = new ClassModel()
            {
                Name = classModel.Name,
                BaseClass = classModel.BaseClass,
                Decorators = new string[] { },
                TypeParameters = classModel.TypeParameters.Select(i => i.Name).ToArray(),
                Imports = classModel.Imports.Select(i => new ImportModel()
                {
                    Names = new string[] { i.Name },
                    Path = i.DependencyKind == DependencyKind.Model
                        ? $"./{NameCaseConverter.ToKebabCase(i.Name)}.model"
                        : $"../enums/{NameCaseConverter.ToKebabCase(i.Name)}.enum"
                }).ToArray(),
                Properties = classModel.Properties.Select(p => new PropertyModel()
                {
                    Name = NameCaseConverter.ToCamelCase(p.Name),
                    Type = typeGenerator.GetEmittedType(p.Type),
                    IsPrivate = false,
                    InitialValue = null,
                }).ToArray(),
            };

            var result = CallGenerator("/generate/class", CreateStringContent(request));
        }

        public void GenerateEnumAST(TypeScriptEnumModel enumModel, string outputPath)
        {
            var request = new EnumGenerationRequest();

            string fileName = $"{NameCaseConverter.ToKebabCase(enumModel.Name)}.enum.{TypeScriptFileExtension.File}";

            request.OutputPath = Path.Combine(outputPath, "enums", fileName);

            request.DataModel = new EnumModel()
            {
                Name = enumModel.Name,
                Members = enumModel.Members.Select(m => new EnumMemberModel()
                {
                    Name = m.Name,
                    Value = m.Value?.ToString()
                }).ToArray()
            };

            var result = CallGenerator("/generate/enum", CreateStringContent(request));
        }
    }
}
