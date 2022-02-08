﻿using Microsoft.Data.Sqlite;
using ProyectoFinalDint.modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalDint.servicios
{
    class SQLiteRepositoryClientes
    {
        private String src = "";
        private String nombreDB = "parking.db";
        private SqliteConnection conexion;
        private SqliteCommand comando;
        public SQLiteRepositoryClientes()
        {
            this.conexion = new SqliteConnection("Data Source=" + this.nombreDB);
        }
        public SQLiteRepositoryClientes(String nombredb, String source)
        {
            this.nombreDB = nombredb;
            this.src = source;
            this.conexion = new SqliteConnection("Data Source=" + this.src + this.nombreDB);
        }

        public void Inserta(Clientes cliente)
        {
            this.conexion.Open();
            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT COUNT(*) FROM clientes";
            int resultado = int.Parse(comando.ExecuteScalar().ToString());

            comando.CommandText = "INSERT INTO clientes VALUES (@id,@nombre,@documento,@foto,@edad,@genero,@telefono)";
            comando.Parameters.Add("@id", SqliteType.Integer);
            comando.Parameters.Add("@nombre", SqliteType.Text);
            comando.Parameters.Add("@documento", SqliteType.Text);
            comando.Parameters.Add("@foto", SqliteType.Text);
            comando.Parameters.Add("@edad", SqliteType.Integer);
            comando.Parameters.Add("@genero", SqliteType.Text);
            comando.Parameters.Add("@telefono", SqliteType.Text);

            comando.Parameters["@id"].Value = resultado;
            comando.Parameters["@nombre"].Value = cliente.Nombre;
            comando.Parameters["@documento"].Value = cliente.Documento;
            comando.Parameters["@foto"].Value = cliente.Foto;
            comando.Parameters["@edad"].Value = cliente.Edad;
            comando.Parameters["@genero"].Value = cliente.Genero;
            comando.Parameters["@telefono"].Value = cliente.Telefono;

            comando.ExecuteNonQuery();
            this.conexion.Close();
        }
        private Clientes ClientesFactory(SqliteDataReader lector) => new Clientes(
            lector.GetInt32(0),
            lector[3].ToString(),
            lector[1].ToString(),
            lector[5].ToString(),
            lector[2].ToString(),
            lector[6].ToString(),
            lector.IsDBNull(4) ? 0 : lector.GetInt32(4)
        );
        public Clientes FindById(int id)
        {
            this.conexion.Open();

            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM clientes WHERE id_cliente = " + id;

            SqliteDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                if (lector.Read())
                {
                    Clientes c = ClientesFactory(lector);
                    this.conexion.Close();
                    lector.Close();
                    return c;
                }
            }
            this.conexion.Close();
            lector.Close();
            return null;
        }
        public Clientes FindByDocumento(String doc)
        {
            this.conexion.Open();

            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM clientes WHERE documento = " + doc;

            SqliteDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                if (lector.Read()) return ClientesFactory(lector);
            }
            this.conexion.Close();
            lector.Close();
            return null;
        }
        public ObservableCollection<Clientes> FindAll()
        {
            this.conexion.Open();

            ObservableCollection<Clientes> clientes = new ObservableCollection<Clientes>();

            comando = conexion.CreateCommand();
            comando.CommandText = "SELECT * FROM clientes";

            SqliteDataReader lector = comando.ExecuteReader();
            if (lector.HasRows)
            {
                while (lector.Read())
                {
                    clientes.Add(ClientesFactory(lector));
                }
           
            }
            this.conexion.Close();
            lector.Close();
            return clientes;
        }
        public bool UpdateClienteEdadGenero(int edad, String genero, int id)
        {
            if (FindById(id) != null)
            {
                this.conexion.Open();
                comando = conexion.CreateCommand();

                comando.CommandText = "UPDATE clientes " +
                    "SET edad = @ed, genero = @gen " +
                    "Where id_cliente = @id";

                comando.Parameters.Add("@gen", SqliteType.Text);
                comando.Parameters.Add("@ed", SqliteType.Integer);
                comando.Parameters.Add("@id", SqliteType.Integer);

                comando.Parameters["@ed"].Value = edad;
                comando.Parameters["@gen"].Value = genero;
                comando.Parameters["@id"].Value = id;

                comando.ExecuteReader();
                this.conexion.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateCliente(Clientes cliente)
        {
            if (FindById(cliente.Id_cliente) != null)
            {
                this.conexion.Open();
                comando = conexion.CreateCommand();

                comando.CommandText = "UPDATE clientes " +
                    "SET nombre = @nom, documento = @doc, foto = @foto, " +
                    "edad = @ed, genero = @gen, telefono = @tel " +
                    "Where id_cliente = @id";

                comando.Parameters.Add("@nom", SqliteType.Text);
                comando.Parameters.Add("@doc", SqliteType.Text);
                comando.Parameters.Add("@foto", SqliteType.Text);
                comando.Parameters.Add("@ed", SqliteType.Integer);
                comando.Parameters.Add("@gen", SqliteType.Text);
                comando.Parameters.Add("@tel", SqliteType.Text);
                comando.Parameters.Add("@id", (SqliteType)1);


                comando.Parameters["@nom"].Value = cliente.Nombre;
                comando.Parameters["@doc"].Value = cliente.Documento;
                comando.Parameters["@foto"].Value = cliente.Foto;
                comando.Parameters["@ed"].Value = cliente.Edad;
                comando.Parameters["@gen"].Value = cliente.Genero;
                comando.Parameters["@tel"].Value = cliente.Telefono;
                comando.Parameters["@id"].Value = cliente.Id_cliente;

                comando.ExecuteReader();
                this.conexion.Close();
                return true;
            }
            else return false;
        }

        public bool DeleteClienteById(int id)
        {
            if (FindById(id) != null)
            {
                this.conexion.Open();
                comando = conexion.CreateCommand();

                comando.CommandText = "DELETE FROM clientes WHERE id_cliente = @id";

                comando.Parameters.Add("@id", SqliteType.Integer);

                comando.Parameters["@id"].Value = id;

                comando.ExecuteNonQuery();
                this.conexion.Close();
                return true;
            }
            else
            {
                this.conexion.Close();
                return false;
            }
        }
        public bool DeleteCliente(Clientes cliente) => DeleteClienteById(cliente.Id_cliente);
    }
}
