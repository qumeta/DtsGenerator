﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtsGenerator.Models
{
    public class DependantType : IComparable<DependantType>
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public string ContainingAssembly { get; set; }

        public bool IsExternal { get; set; }

        public SemanticModel SemanticModel { get; set; }

        public INamedTypeSymbol NamedTypeSymbol { get; set; }

        public TypeKind TypeKind { get; set; }

        public override string ToString()
        {
            return $"{Namespace}.{Name} [{ContainingAssembly}]";
        }

        public int CompareTo(DependantType other)
        {
            return this.ToString().CompareTo(other.ToString());
        }
    }
}
