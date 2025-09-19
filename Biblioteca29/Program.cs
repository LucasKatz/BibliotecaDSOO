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
        public bool Prestado { get; set; } = false;  // Al estar en la biblioteca, todo libro agregado empieza en FALSE
                                                     

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
        public List<Libro> libros;

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
    } // OJO, A PARTIR DE ACÁ, YENDO HACIA ARRIBA, ES COPIA DE COSAS DE LA CLASE 

    // Clase Lectores
    public class Lector
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public int PrestamosActivos { get; set; }
        public List<Libro> Prestamos { get; set; } // Lista de libros prestados

        public Lector(string nombre, string apellido, int dni, int prestamosActivos)
        {
            Nombre = nombre;
            Apellido = apellido;
            DNI = dni;
            PrestamosActivos = prestamosActivos;
            Prestamos = new List<Libro>(); // Inicializamos lista vacía
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

        public string DevolverLibro(Biblioteca biblioteca, string titulo, int dni)
        {
            Lector lector = BuscarLector(dni);
            if (lector == null)
                return "Lector inexistente";

            // Buscar libro en la lista de préstamos del lector
            Libro libro = lector.Prestamos.Find(l => l.Titulo == titulo);
            if (libro == null)
                return "Ese libro no está en préstamo por este lector";

            // Marcar como disponible
            libro.Prestado = false;

            // Eliminar de la lista de préstamos del lector
            lector.Prestamos.Remove(libro);

            // Disminuir el contador (solo si es > 0)
            if (lector.PrestamosActivos > 0)
                lector.PrestamosActivos--;

            // Devolver a la biblioteca (usando la firma existente)
            biblioteca.AgregarLibro(libro.Titulo, libro.Autor, libro.Editorial);

            return "Devolución exitosa";
        }






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

            // Marcar como prestado
            libro.Prestado = true;

            // Agregar a la lista de préstamos del lector
            lector.Prestamos.Add(libro);

            // Aumentar el contador
            lector.PrestamosActivos++;

            // Eliminar de la biblioteca
            biblioteca.EliminarLibro(titulo);

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

            // Agregar libros iniciales
            biblioteca.AgregarLibro("Libro1", "Autor1", "Editorial1");
            biblioteca.AgregarLibro("Libro2", "Autor2", "Editorial2");
            biblioteca.AgregarLibro("Libro3", "Autor3", "Editorial3");
            biblioteca.AgregarLibro("Libro4", "Autor4", "Editorial4");
            biblioteca.AgregarLibro("Libro5", "Autor5", "Editorial5");
            biblioteca.AgregarLibro("Libro6", "Autor6", "Editorial6");

            Console.WriteLine("Libros en la biblioteca:");
            biblioteca.ListarLibros();

            // Crear registro de lectores (todos con 0 préstamos activos al inicio)
            LectoresRegistrados registro = new LectoresRegistrados();
            Console.WriteLine(registro.RegistrarLector("Juan", "Perez", 12345678, 0));
            Console.WriteLine(registro.RegistrarLector("Ana", "Gomez", 87654321, 0));
            Console.WriteLine(registro.RegistrarLector("Pepe", "Lopez", 22222222, 0));

            // ---- Preparación: prestarle 3 libros a Ana para que alcance el máximo ----
            Console.WriteLine("\n=== Preparación: prestarle 3 libros a Ana (87654321) ===");
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro2", 87654321)); // Préstamo exitoso
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro3", 87654321)); // Préstamo exitoso
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro4", 87654321)); // Préstamo exitoso

            Console.WriteLine("\nEstado biblioteca después de pre-préstamos:");
            biblioteca.ListarLibros();

            // ---- Tests PrestarLibro: cubrir todos los resultados posibles ----
            Console.WriteLine("\n=== Tests PrestarLibro ===");

            // 1) Préstamo exitoso
            Console.WriteLine("\n[Caso] Préstamo exitoso (Juan pide Libro1):");
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro1", 12345678)); // Préstamo exitoso

            // 2) Intentar prestar libro que está marcado como Prestado (simulamos un libro con Prestado = true)
            Console.WriteLine("\n[Caso] Intentar prestar libro ya prestado (simulado):");
            biblioteca.AgregarLibro("LibroPrestado", "AutorX", "EdX"); // lo agregamos
            var bFake = biblioteca.BuscarLibro("LibroPrestado");
            if (bFake != null) bFake.Prestado = true; // lo marcamos como prestado sin quitarlo de la biblioteca
            Console.WriteLine(registro.PrestarLibro(biblioteca, "LibroPrestado", 12345678)); // debe devolver "El libro ya está prestado"

            // 3) Libro inexistente
            Console.WriteLine("\n[Caso] Libro inexistente:");
            Console.WriteLine(registro.PrestarLibro(biblioteca, "LibroNoExiste", 12345678));

            // 4) Lector inexistente
            Console.WriteLine("\n[Caso] Lector inexistente:");
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro5", 99999999));

            // 5) Lector alcanzó máximo de préstamos activos (Ana ya tiene 3)
            Console.WriteLine("\n[Caso] Lector alcanzó máximo de préstamos activos (Ana tiene 3):");
            Console.WriteLine(registro.PrestarLibro(biblioteca, "Libro5", 87654321));

            // ---- Tests DevolverLibro: cubrir todos los resultados posibles ----
            Console.WriteLine("\n=== Tests DevolverLibro ===");

            // 1) Devolución exitosa (Ana devuelve uno de sus libros)
            Console.WriteLine("\n[Caso] Devolución exitosa (Ana devuelve Libro2):");
            Console.WriteLine(registro.DevolverLibro(biblioteca, "Libro2", 87654321)); // Devolución exitosa

            // 2) Intentar devolver mismo libro otra vez (ya devuelto)
            Console.WriteLine("\n[Caso] Intentar devolver mismo libro otra vez (ya devuelto):");
            Console.WriteLine(registro.DevolverLibro(biblioteca, "Libro2", 87654321)); // Ese libro no está en préstamo por este lector

            // 3) Lector inexistente en devolución
            Console.WriteLine("\n[Caso] Lector inexistente en devolución:");
            Console.WriteLine(registro.DevolverLibro(biblioteca, "Libro6", 99999999)); // Lector inexistente

            // 4) Devolver libro que el lector no tiene (Pepe intenta devolver Libro3)
            Console.WriteLine("\n[Caso] Devolver libro que el lector no tiene (Pepe intenta devolver Libro3):");
            Console.WriteLine(registro.DevolverLibro(biblioteca, "Libro3", 22222222)); // Ese libro no está en préstamo por este lector

            // ---- Tests de AgregarLibro / EliminarLibro ----
            Console.WriteLine("\n=== Tests AgregarLibro / EliminarLibro ===");

            // Intentar agregar duplicado (Libro5 ya existe)
            Console.WriteLine("\n[Caso] Agregar libro con título existente (Libro5):");
            bool agregado = biblioteca.AgregarLibro("Libro5", "Autor5", "Editorial5");
            Console.WriteLine(agregado ? "Agregado" : "No agregado (ya existe)");

            // Eliminar libro existente (Libro6)
            Console.WriteLine("\n[Caso] Eliminar libro existente (Libro6):");
            Console.WriteLine(biblioteca.EliminarLibro("Libro6") ? "Eliminado" : "No eliminado");

            // Eliminar libro inexistente
            Console.WriteLine("\n[Caso] Eliminar libro inexistente (NoExiste):");
            Console.WriteLine(biblioteca.EliminarLibro("NoExiste") ? "Eliminado" : "No eliminado");

            // ---- Estado final ----
            Console.WriteLine("\nEstado final de la biblioteca:");
            biblioteca.ListarLibros();

            Console.WriteLine("\nEstado de lectores (PrestamosActivos y títulos):");
            var juan = registro.BuscarLector(12345678);
            var ana = registro.BuscarLector(87654321);
            var pepe = registro.BuscarLector(22222222);

            Console.WriteLine($"Juan: PrestamosActivos={juan.PrestamosActivos}, Prestamos=[{string.Join(", ", juan.Prestamos.ConvertAll(l => l.Titulo))}]");
            Console.WriteLine($"Ana: PrestamosActivos={ana.PrestamosActivos}, Prestamos=[{string.Join(", ", ana.Prestamos.ConvertAll(l => l.Titulo))}]");
            Console.WriteLine($"Pepe: PrestamosActivos={pepe.PrestamosActivos}, Prestamos=[{string.Join(", ", pepe.Prestamos.ConvertAll(l => l.Titulo))}]");

            Console.ReadLine();
        }
    }


}
