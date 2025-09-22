using System;
using System.Collections.Generic;

namespace Colecciones
{
    // Clase Libro
    public class Libro
    {
        public string titulo { get; set; }
        public string autor { get; set; }
        public string editorial { get; set; }
        public bool prestado { get; set; } = false;

        public Libro(string titulo, string autor, string editorial)
        {
            this.titulo = titulo;
            this.autor = autor;
            this.editorial = editorial;
        }

        public override string ToString()
        {
            return $"Título: {titulo}, Autor: {autor}, Editorial: {editorial}, Prestado: {prestado}";
        }
    }

    // Clase Lector
    public class Lector
    {
        private string nombreCompleto;
        private int dni;
        public List<Libro> prestamos;

        public Lector(string nombreCompleto, int dni)
        {
            this.nombreCompleto = nombreCompleto;
            this.dni = dni;
            this.prestamos = new List<Libro>();
        }

        public string getNombre() => nombreCompleto;
        public int getDNI() => dni;
        public bool puedePedir() => prestamos.Count < 3;

        public string devolverLibro(Biblioteca biblioteca, string titulo)
        {
            Libro libro = prestamos.Find(l => l.titulo == titulo);
            if (libro == null)
                return "El lector no tiene ese libro prestado";

            libro.prestado = false;
            prestamos.Remove(libro);
            return "Devolución exitosa";
        }

        public void listarPrestamos()
        {
            if (prestamos.Count == 0)
            {
                Console.WriteLine("  - No tiene libros en préstamo");
                return;
            }
            foreach (var libro in prestamos)
            {
                Console.WriteLine("  - " + libro);
            }
        }
    }

    // Clase Biblioteca
    public class Biblioteca
    {
        private string nombre;
        public List<Libro> libros;
        public List<Lector> lectores;

        public Biblioteca(string nombre)
        {
            this.nombre = nombre;
            this.libros = new List<Libro>();
            this.lectores = new List<Lector>();
        }

        // Alta de lector
        public string altaLector(string nombreCompleto, int dni)
        {
            if (lectores.Exists(l => l.getDNI() == dni))
                return "LECTOR YA REGISTRADO";

            lectores.Add(new Lector(nombreCompleto, dni));
            return "LECTOR REGISTRADO";
        }

        private Lector buscarLector(int dni) => lectores.Find(l => l.getDNI() == dni);
        private Libro buscarLibro(string titulo) => libros.Find(l => l.titulo == titulo);

        // Prestar libro
        public string prestarLibro(string titulo, int dniLector)
        {
            Lector lector = buscarLector(dniLector);
            if (lector == null) return "LECTOR INEXISTENTE";

            Libro libro = buscarLibro(titulo);
            if (libro == null) return "LIBRO INEXISTENTE";
            if (libro.prestado) return "LIBRO YA PRESTADO";

            if (!lector.puedePedir()) return "TOPE DE PRESTAMO ALCANZADO";

            libro.prestado = true;
            lector.prestamos.Add(libro);
            return "PRESTAMO EXITOSO";
        }

        // Agregar libro
        public string agregarLibro(string titulo, string autor, string editorial)
        {
            if (buscarLibro(titulo) != null)
                return "LIBRO YA EXISTE";

            libros.Add(new Libro(titulo, autor, editorial));
            return "LIBRO AGREGADO";
        }

        // Eliminar libro
        public bool eliminarLibro(string titulo)
        {
            Libro libro = buscarLibro(titulo);
            if (libro == null) return false;

            libros.Remove(libro);
            return true;
        }

        // Listar libros
        public void listarLibros()
        {
            Console.WriteLine("Libros en la biblioteca " + nombre + ":");
            foreach (var libro in libros)
                Console.WriteLine("  - " + libro);
        }
    }

    // Clase de prueba
    class Test
    {
        static void Main(string[] args)
        {
            Biblioteca biblioteca29 = new Biblioteca("Biblioteca 29");

            // Agregar libros
            biblioteca29.agregarLibro("El Aleph", "Jorge Luis Borges", "Emecé");
            biblioteca29.agregarLibro("Rayuela", "Julio Cortázar", "Sudamericana");
            biblioteca29.agregarLibro("El resplandor", "Stephen King", "Doubleday");
            biblioteca29.agregarLibro("Fundación", "Isaac Asimov", "Gnome Press");
            biblioteca29.agregarLibro("Rebelión en la granja", "George Orwell", "Secker & Warburg");
            biblioteca29.agregarLibro("Cien años de soledad", "Gabriel García Márquez", "Sudamericana");

            biblioteca29.listarLibros();

            // Crear lectores
            Console.WriteLine(biblioteca29.altaLector("Juan Perez", 30123456));
            Console.WriteLine(biblioteca29.altaLector("Ana Gomez", 30876543));
            Console.WriteLine(biblioteca29.altaLector("Pepe Lopez", 31543210));

            // Préstamos iniciales
            biblioteca29.prestarLibro("Rayuela", 30876543);
            biblioteca29.prestarLibro("El resplandor", 30876543);
            biblioteca29.prestarLibro("Fundación", 30876543);

            Console.WriteLine("Estado biblioteca después de pre-préstamos:");
            biblioteca29.listarLibros();

            // Pruebas PrestarLibro
            Console.WriteLine("Préstamo exitoso (Juan Perez pide El Aleph): " + biblioteca29.prestarLibro("El Aleph", 30123456));
            Console.WriteLine("Intentar prestar libro inexistente (Juan Perez pide LibroNoExiste): " + biblioteca29.prestarLibro("LibroNoExiste", 30123456));
            Console.WriteLine("Lector inexistente (DNI 39999999 pide Rebelión en la granja): " + biblioteca29.prestarLibro("Rebelión en la granja", 39999999));
            Console.WriteLine("Lector con tope de préstamos (Ana Gomez pide Cien años de soledad): " + biblioteca29.prestarLibro("Cien años de soledad", 30876543));

            // Pruebas DevolverLibro
            Lector ana = biblioteca29.lectores.Find(l => l.getDNI() == 30876543);
            Lector pepe = biblioteca29.lectores.Find(l => l.getDNI() == 31543210);

            Console.WriteLine("Devolución exitosa (Ana Gomez devuelve Rayuela): " + ana.devolverLibro(biblioteca29, "Rayuela"));
            Console.WriteLine("Intentar devolver mismo libro otra vez (Ana Gomez devuelve Rayuela): " + ana.devolverLibro(biblioteca29, "Rayuela"));
            Console.WriteLine("Devolver libro que el lector no tiene (Pepe Lopez intenta El resplandor): " + pepe.devolverLibro(biblioteca29, "El resplandor"));

            // Estado final
            Console.WriteLine("--- Estado final de la biblioteca ---");
            biblioteca29.listarLibros();

            Console.WriteLine("--- Estado de lectores ---");
            foreach (var lector in biblioteca29.lectores)
            {
                Console.WriteLine($"{lector.getNombre()} (DNI: {lector.getDNI()}): Prestamos=[{string.Join(", ", lector.prestamos.ConvertAll(l => l.titulo))}]");
            }

            Console.ReadLine();
        }
    }
}