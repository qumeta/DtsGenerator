using DtsGenerator.Constants;
using DtsGenerator.Extensions;
using DtsGenerator.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DtsGenerator.Emitters
{
    public class TypeScriptEmitter
    {
        public void Emit(string path, string name, EmittedFileType fileType, string contents)
        {
            string fileTypeName = fileType.GetDescription();
            string fileTypeDirectoryName = MapFileTypeToDirectoryName(fileType);

            string directory = Path.Combine(path, fileTypeDirectoryName);

            // this will also ensure the root directory is created if it does not exist yet
            Directory.CreateDirectory(directory);

            string fileName = $"{NameCaseConverter.ToKebabCase(name)}.{fileTypeName}.{TypeScriptFileExtension.File}";

            File.WriteAllText(Path.Combine(directory, fileName), contents);
        }

        public void EmitAll(string path, EmittedFileType fileType, string contents)
        {
            string fileTypeDirectoryName = MapFileTypeToDirectoryName(fileType);

            // this will also ensure the root directory is created if it does not exist yet
            Directory.CreateDirectory(path);

            string fileName = $"{NameCaseConverter.ToKebabCase(fileTypeDirectoryName)}.{TypeScriptFileExtension.File}";

            File.WriteAllText(Path.Combine(path, fileName), contents);
        }

        private string MapFileTypeToDirectoryName(EmittedFileType fileType)
        {
            switch (fileType)
            {
                case EmittedFileType.Model:
                    return EmittedDirectoryName.Models;
                case EmittedFileType.Enum:
                    return EmittedDirectoryName.Enums;
                case EmittedFileType.Service:
                    return EmittedDirectoryName.Services;
                default:
                    return string.Empty;
            }
        }
    }
}
