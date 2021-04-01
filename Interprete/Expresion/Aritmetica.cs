﻿using Proyecto1.Codigo3D;
using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1.Interprete.Expresion
{
    class Aritmetica : Expresion
    {
        public Expresion izquierda;
        public Expresion derecha;
        public char tipo;

        public Aritmetica(Expresion izquierda, Expresion derecha, char tipo)
        {
            this.izquierda = izquierda;
            this.derecha = derecha;
            this.tipo = tipo;
        }
        public override Simbolo Evaluar(TabladeSimbolos ts)
        {
            Simbolo izquierda = this.izquierda.Evaluar(ts);
            Simbolo derecha = null;
            Simbolo resultado;
            Tipos tipoResultante;
            if (this.derecha != null)
            {
                derecha = this.derecha.Evaluar(ts);
                tipoResultante = TablaTipos.getTipo(izquierda.Tipo, derecha.Tipo);
            }
            else 
            {
                tipoResultante = TablaTipos.getTipo(izquierda.Tipo, izquierda.Tipo);
            }
            if ((int)tipoResultante==7)
                throw new Exception();

            switch (tipo)
            {
                case '+':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower()) 
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) + int.Parse(derecha.Value.ToString());
                            break;
                        case "string":
                            resultado.Value = izquierda.Value.ToString() + derecha.Value.ToString();
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) + decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case '-':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    if (this.derecha == null)
                    {
                        switch (tipoResultante.ToString().ToLower())
                        {
                            case "int":
                                resultado.Value = - int.Parse(izquierda.Value.ToString());
                                break;
                            case "real":
                                resultado.Value = - decimal.Parse(izquierda.Value.ToString());
                                break;
                        }

                    }
                    else 
                    {
                        switch (tipoResultante.ToString().ToLower())
                        {
                            case "int":
                                resultado.Value = int.Parse(izquierda.Value.ToString()) - int.Parse(derecha.Value.ToString());
                                break;
                            case "real":
                                resultado.Value = decimal.Parse(izquierda.Value.ToString()) - decimal.Parse(derecha.Value.ToString());
                                break;
                        }
                    }
                    return resultado;
                case '*':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) * int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) * decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case '/':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) / decimal.Parse(derecha.Value.ToString());
                            resultado.Tipo = new Tipo(Tipos.REAL, "real");
                            break;
                    }
                    return resultado;
                case 'd':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) / int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) / decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                case 'm':
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) % int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) % decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
                default:
                    resultado = new Simbolo(null, izquierda.Tipo, 0, 0, false);
                    switch (tipoResultante.ToString().ToLower())
                    {
                        case "int":
                            resultado.Value = int.Parse(izquierda.Value.ToString()) % int.Parse(derecha.Value.ToString());
                            break;
                        case "real":
                            resultado.Value = decimal.Parse(izquierda.Value.ToString()) % decimal.Parse(derecha.Value.ToString());
                            break;
                    }
                    return resultado;
            }
        }

        public override string generar3D(TabladeSimbolos ts, Intermedio c3d)
        {
            string code = "";
            if (this.derecha == null)
            {
                
            }
            else 
            {
                if (this.izquierda is Primitivo & this.derecha is Primitivo)
                {
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += this.izquierda.generar3D(ts, c3d);
                    code += this.tipo.ToString();
                    code += this.derecha.generar3D(ts, c3d);
                }
                else if (this.izquierda is Primitivo & !(this.derecha is Primitivo))
                {
                    string tmp = "";
                    code += this.derecha.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += this.izquierda.generar3D(ts, c3d);
                    code += this.tipo.ToString();
                    code += tmp;
                }
                else if (!(this.izquierda is Primitivo) & this.derecha is Primitivo)
                {
                    string tmp = "";
                    code += this.izquierda.generar3D(ts, c3d);
                    tmp = c3d.tmp.getLastTemporal();
                    code += c3d.tmp.generarTemporal() + " = ";
                    code += tmp;
                    code += this.tipo.ToString();
                    code += this.derecha.generar3D(ts, c3d);
                }
                else
                {
                    Aritmetica izq = (Aritmetica)this.izquierda;
                    Aritmetica der = (Aritmetica)this.derecha;
                    if (izq.tipo == '*' || izq.tipo == '/' || izq.tipo == '%') 
                    {
                        code += izq.generar3D(ts, c3d);
                        code += der.generar3D(ts, c3d);
                    }
                    if (der.tipo == '*' || der.tipo == '/' || der.tipo == '%')
                    {
                        code += der.generar3D(ts, c3d);
                        code += izq.generar3D(ts, c3d);
                    }
                }
            }
            
            return code + ";\n";
        }
    }
}
