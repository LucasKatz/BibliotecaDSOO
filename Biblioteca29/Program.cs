using System;
using System.Collections.Generic;

namespace Colecciones
{
    // Clase Domicilio
    public class Domicilio
    {
        public string Calle { get; set; }
        public int Numero { get; set; }


        public Domicilio(string calle, int numero)
        {
            Calle = calle;
            Numero = numero;
        }
    }

    // Clase Libro
    public class Libro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public bool Prestado { get; set; } = false;

        public Libro(string titulo, string autor, string editorial)
        {
            Titulo = titulo;
            Autor = autor;
            Editorial = editorial;
        }

        public override string ToString()
        {
            return $"Título: {Titulo}, Autor: {Autor}, Editorial: {Editorial}, Prestado: {Prestado}";
        }
    }

    // Clase Biblioteca (contiene funciones que administran los libros)
    public class Biblioteca
    {
        public string Nombre { get; set; }
        public Domicilio Domicilio { get; set; }
        private List<Libro> libros;

        public Biblioteca(string nombre, Domicilio domicilio)
        {
            Nombre = nombre;
            Domicilio = domicilio;
            libros = new List<Libro>();
        }

        public bool AgregarLibro(string titulo, string autor, string editorial)
        {
            if (BuscarLibro(titulo) == null)
            {
                libros.Add(new Libro(titulo, autor, editorial));
                return true;
            }
            return false;
        }

        public bool EliminarLibro(string titulo)
        {
            Libro libro = BuscarLibro(titulo);
            if (libro != null)
            {
                libros.Remove(libro);
                return true;
            }
            return false;
        }

        public void ListarLibros()
        {
            foreach (var libro in libros)
                Console.WriteLine(libro);
        }

        public Libro BuscarLibro(string titulo)
        {
            foreach (var libro in libros)
            {
                if (libro.Titulo.Equals(titulo))
                    return libro;
            }
            return null;
        }
    }

    // Clase Lectores
    public class Lector
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public int PrestamosActivos { get; set; }
        public Lector(string nombre, string apellido, int dni, int prestamosActivos)
        {
            Nombre = nombre;
            Apellido = apellido;
            DNI = dni;
            PrestamosActivos = prestamosActivos;
        }
    }

    // Clase que administra los lectores registrados
    public class LectoresRegistrados
    {
        private List<Lector> lectores;

        public LectoresRegistrados()
        {
            lectores = new List<Lector>();
        }

        public string RegistrarLector(string nombre, string apellido, int dni, int prestamosActivos)
        {
            foreach (var lector in lectores)
            {
                if (lector.DNI == dni)
                    return "El lector ya está registrado.";
            }

            lectores.Add(new Lector(nombre, apellido, dni, prestamosActivos));
            return "Lector registrado correctamente.";
        }

        //Función para buscar si el lector existe
        public Lector BuscarLector(int dni)
        {
            foreach (var lector in lectores)
            {
                if (lector.DNI == dni)
                    return lector;
            }
            return null;
        }


        /*public bool CantidadPrestamos(int prestamosActivos)
        {
            return prestamosActivos < 3;
        }*/



        // Función para prestar libro
        public string PrestarLibro(Biblioteca biblioteca, string titulo, int dni)
        {
            Lector lector = BuscarLector(dni);
            if (lector == null)
                return "Lector inexistente";

            Libro libro = biblioteca.BuscarLibro(titulo);
            if (libro == null)
                return "Libro inexistente";

            if (libro.Prestado)
                return "El libro ya está prestado";

            if (lector.PrestamosActivos >= 3)
                return "Alcanzó el máximo de préstamos activos";

            libro.Prestado = true;
            lector.PrestamosActivos ++;
            return "Préstamo exitoso";
        }
    }

    // Pruebas a ver si el codigo funciona
    class Test
    {
        static void Main(string[] args)
        {
            // Crear biblioteca
            Domicilio dom = new Domicilio("Calle Falsa", 123);
            Biblioteca biblioteca = new Biblioteca("Biblio29", dom);

            // Agregar libros
            biblioteca.AgregarLibro("Libro1", "Autor1", "Editorial1");
            biblioteca.AgregarLibro("Libro2", "Autor2", "Editorial2");

            // Listar libros
            Console.WriteLine("Libros en la biblioteca:");
            biblioteca.ListarLibros();

            // Crear registro de lectores
            LectoresRegistrados registro = new LectoresRegistrados();

            // Registrar lectores
            Console.WriteLine(registro.RegistrarLector("Juan", "Perez", 12345678, 0));
            Console.WriteLine(registro.RegistrarLector("Ana", "Gomez", 87654321, 3));

            // Probar préstamos
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro1", 12345678));
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro1", 12345678));
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro3", 12345678));
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro2", 99999999));
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro2", 87654321));


            // Listar libros para ver el estado de préstamo
            Console.WriteLine("\nEstado de los libros después de los préstamos:");
            biblioteca.ListarLibros();

            Console.ReadLine(); // para que la consola no se cierre de inmediato
        }
    }

}
