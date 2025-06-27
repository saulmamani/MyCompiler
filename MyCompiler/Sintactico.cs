using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompiler
{
    class Sintactico
    {
        public List<string> Errors;


        public bool Analizar(List<Simbolo> tablaSimbolos)
        {
            Errors = new List<string>();

            int index = 0;
            return MiPrograma(tablaSimbolos, ref index) && index == (tablaSimbolos.Count - 1);
        }

        private bool MiPrograma(List<Simbolo> ts, ref int index)
        {
            if (ts[index].Lexema == "void")
            {
                index++;
                if (ts[index].Lexema == "main")
                {
                    index++;
                    if (ts[index].Token == "ParenAbre")
                    {
                        index++;
                        if (ts[index].Token == "ParenCierre")
                        {
                            index++;
                            if (ts[index].Token == "LlaveAbre")
                            {
                                index++;
                                if (Cuerpo(ts, ref index))
                                {
                                    if (ts[index].Token == "LlaveCierre")
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        Errors.Add($"Error ({ts[index].Linea}): -> Se esperaba llave de cierre");
                                        return false;
                                    }
                                }
                                return false;
                            }
                            else
                            {
                                Errors.Add($"Error ({ts[index].Linea}): -> Se esperaba llave de apertura");
                                return false;
                            }

                        }
                        else
                        {
                            Errors.Add($"Error ({ts[index].Linea}): -> Se esperaba ')'");
                            return false;
                        }


                    }
                    else
                    {
                        Errors.Add($"Error ({ts[index].Linea}): -> Se esperaba '('");
                        return false;
                    }
                }
                else
                {
                    Errors.Add($"Error ({ts[index].Linea}): -> Se esperaba 'main'");
                    return false;
                }

            }
            else
            {
                Errors.Add($"Error ({ts[index].Linea}): -> Se esperaba 'void'");
                return false;
            }
        }

        private bool Cuerpo(List<Simbolo> ts, ref int index)
        {
            if (Sentencias(ts, ref index))
            {
                if (ts[index].Token != "LlaveCierre")
                {
                    return Cuerpo(ts, ref index);
                }
                return true;
            }
            return false;
        }

        private bool Sentencias(List<Simbolo> ts, ref int index)
        {
            int currentIndex = index;

            // Primero intentamos con Declaraciones
            if (Declaraciones(ts, ref index))
            {
                return true;
            }

            // Si falla, restauramos el índice y probamos con Asignaciones
            index = currentIndex;
            if (Asignaciones(ts, ref index))
            {
                return true;
            }

            // Si falla, restauramos el índice y probamos con Impresiones
            index = currentIndex;
            if (Impresiones(ts, ref index))
            {
                return true;
            }

            Errors.Add($"Error ({ts[index].Linea}): Se esperaba declaración, asignación o impresión");
            return false;
        }

        private bool Declaraciones(List<Simbolo> ts, ref int index)
        {
            if (Decla(ts, ref index))
            {
                if (ts[index].Token == "Tipo")
                {
                    return Declaraciones(ts, ref index);
                }
                return true;
            }
            return false;
        }

        private bool Decla(List<Simbolo> ts, ref int index)
        {
            if (ts[index].Token == "Tipo")
            {
                index++;
                if (ts[index].Token == "Id")
                {
                    index++;
                    if (ts[index].Lexema == ";")
                    {
                        index++;
                        return true;
                    }
                    Errors.Add($"Error ({ts[index].Linea}): Se esperaba ';'");
                    return false;
                }
                Errors.Add($"Error ({ts[index].Linea}): Se esperaba identificador");
                return false;
            }
            Errors.Add($"Error ({ts[index].Linea}): Se esperaba tipo de dato");
            return false;
        }

        private bool Asignaciones(List<Simbolo> ts, ref int index)
        {
            if (Asig(ts, ref index))
            {
                if (ts[index].Token == "Id" && ts[index + 1].Lexema == "=")
                {
                    return Asignaciones(ts, ref index);
                }
                return true;
            }
            return false;
        }

        private bool Asig(List<Simbolo> ts, ref int index)
        {
            if (ts[index].Token == "Id")
            {
                index++;
                if (ts[index].Lexema == "=")
                {
                    index++;
                    if (Val(ts, ref index))
                    {
                        if (ts[index].Lexema == ";")
                        {
                            index++;
                            return true;
                        }
                        Errors.Add($"Error ({ts[index].Linea}): Se esperaba ';'");
                        return false;
                    }
                    return false;
                }
                Errors.Add($"Error ({ts[index].Linea}): Se esperaba '='");
                return false;
            }
            return false;
        }

        private bool Val(List<Simbolo> ts, ref int index)
        {
            if (ts[index].Token == "Id" || ts[index].Token == "Entero" || ts[index].Token == "Real")
            {
                index++;
                return true;
            }

            Errors.Add($"Error ({ts[index].Linea}): Se esperaba identificador, número entero o real");
            return false;
        }

        private bool Impresiones(List<Simbolo> ts, ref int index)
        {
            if (Imp(ts, ref index))
            {
                if (ts[index].Lexema == "echo")
                {
                    return Impresiones(ts, ref index);
                }
                return true;
            }
            return false;
        }

        private bool Imp(List<Simbolo> ts, ref int index)
        {
            if (ts[index].Lexema == "echo")
            {
                index++;
                if (Val(ts, ref index))
                {
                    if (ts[index].Lexema == ";")
                    {
                        index++;
                        return true;
                    }
                    Errors.Add($"Error ({ts[index].Linea}): Se esperaba ';'");
                    return false;
                }
                return false;
            }
            return false;
        }
    }
}
