﻿using Proyecto1.TS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1.Interprete.Instruccion
{
    class If : Instruccion
    {
        private Expresion.Expresion valor;
        private LinkedList<Instruccion> instrucciones;
        private Instruccion _else;
        List<Object> salida;
        public If(Expresion.Expresion valor, LinkedList<Instruccion> instrucciones, Instruccion _else)
        {
            this.valor = valor;
            this.instrucciones = instrucciones;
            this._else = _else;
            this.salida = new List<Object>();
        }
        public override object Ejecutar(TabladeSimbolos ts)
        {
            Simbolo valor = this.valor.Evaluar(ts);

            //TODO verificar errores
            if (valor.Tipo.tipo != Tipos.BOOLEAN)
                throw new Exception("El tipo no es booleano para el IF");

            if (bool.Parse(valor.Value.ToString()))
            {
                try
                {
                    foreach (var instruccion in instrucciones)
                    {
                        if (instruccion != null)
                        {
                            Object output = instruccion.Ejecutar(ts);
                            if (output is List<Object>)
                            {
                                this.salida.AddRange((List<Object>)output);
                            }
                            else
                            {
                                this.salida.Add(output);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                if (_else != null) this.salida.Add(_else.Ejecutar(ts));
            }
            return this.salida;
        }
    }
}
