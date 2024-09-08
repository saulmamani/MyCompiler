using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyCompiler
{
    internal class Lexico
    {
        public List<string> Errors;
        private List<Simbolo> TablaSimbolos;

        Dictionary<string, string> _regexPatterns = new Dictionary<string, string>
        {
            {"PalabraReservada", @"\b(include|iostream|main|return|endl)\b"},
            {"TipoDato", @"\b(int|float|string|char|void)\b"},
            {"Comentario", @"//.*"},
            {"ComentarioBloque", @"/\*.*?\*/"},
            {"Texto", "\"[^\"]*\""},
            {"Identificador", @"\b_?[a-zA-Z]+_?([a-zA-Z]|\d|_)*\b"},
            {"Numero", @"(?<!\d)-\b\d+\b|\b\d+\b"},
            {"NumeroDecimal", @"(?<!\d)-\b\d+(\.\d+)?\b|\b\d+(\.\d+)?\b"},
            {"EntradaConsola", @">>"},
            {"SalidaConsola", @"<<"},
            {"Condicional", @"==|!=|<=|>="},
            {"MenorQue", @"<"},
            {"MayorQue", @">"},
            {"Simbolos", @"#|,"},
            {"Asignacion", @"="},
            {"Terminador", @"[;]"},
            {"Operadores", @"[\+\-\*/%]"},
            {"ParenAbre",@"[\(]"},
            {"ParenCierre",@"[\)]"},
            {"LlaveAbre", @"{"},
            {"LlaveCierre", @"}"}
        };

        public List<Simbolo> Analizar(string sourceCode)
        {
            // Instanciando las propiedades
            Errors = new List<string>();
            TablaSimbolos = new List<Simbolo>();

            // Separando el codigo fuente en lineas (fila por fila)
            string[] lines = sourceCode.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Uniendo las expresiones regulares
            string pattern = string.Join("|", _regexPatterns.Values);
            Regex regex = new Regex(pattern);

            int lineNumber = 1;
            foreach (string line in lines)
            {
                int currentIndex = 0;
                // Evaluando las expresiones regulares (linea por linea)
                MatchCollection matches = regex.Matches(line);

                foreach (Match match in matches)
                {
                    string value = match.Value;
                    int matchStartIndex = match.Index;

                    // Detectar si hay texto no reconocido antes de este match
                    if (currentIndex < matchStartIndex)
                    {
                        string unrecognized = line.Substring(currentIndex, matchStartIndex - currentIndex);
                        if (!string.IsNullOrWhiteSpace(unrecognized))
                        {
                            Errors.Add($"Error ({lineNumber}:{currentIndex + 1}): {unrecognized} -> No se reconce la cadena");
                        }
                    }

                    // Anadiendo el lexema y el token en la tabla de simbolos, excluyendo los comentarios
                    string tokenName = GetToken(value);
                    if (tokenName != "Comentario" && tokenName != "ComentarioBloque")
                    {
                        TablaSimbolos.Add(new Simbolo
                        {
                            Lexema = value,
                            Token = tokenName,
                            Linea = lineNumber,
                            Columna = match.Index + 1
                        });
                    }

                    // Actualizar el índice actual
                    currentIndex = match.Index + match.Length;
                }

                // Detectar si hay texto no reconocido después del último match
                if (currentIndex < line.Length)
                {
                    string unrecognized = line.Substring(currentIndex);
                    if (!string.IsNullOrWhiteSpace(unrecognized))
                    {
                        Errors.Add($"Error ({lineNumber}:{currentIndex + 1}): {unrecognized} -> No se reconce la cadena");
                    }
                }

                lineNumber++;
            }

            return TablaSimbolos;
        }

        private string GetToken(string value)
        {
            string tokenName = _regexPatterns.FirstOrDefault(x => Regex.IsMatch(value, x.Value)).Key;
            return tokenName;
        }
    }
}
