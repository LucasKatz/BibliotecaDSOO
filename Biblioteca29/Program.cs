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
        public string titulo { get; set; }
        public string autor { get; set; }
        public string editorial { get; set; }
        public bool prestado { get; set; } = false;  // Al estar en la biblioteca, todo libro agregado empieza en FALSE
                                                     

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

    // Clase Biblioteca (contiene funciones que administran los libros)
    public class Biblioteca
    {
        public string nombre { get; set; }
        public Domicilio domicilio { get; set; }
        public List<Libro> libros;

        public Biblioteca(string nombre, Domicilio domicilio)
        {
            this.nombre = nombre;
            this.domicilio = domicilio;
            this.libros = new List<Libro>();
        }

        public bool agregarLibro(string titulo, string autor, string editorial)
        {
            if (buscarLibro(titulo) == null)
            {
                libros.Add(new Libro(titulo, autor, editorial));
                return true;
            }
            return false;
        }

        public bool eliminarLibro(string titulo)
        {
            Libro libro = buscarLibro(titulo);
            if (libro != null)
            {
                libros.Remove(libro);
                return true;
            }
            return false;
        }

        public void listarLibros()
        {
            foreach (var libro in libros)
                Console.WriteLine(libro);
        }

        public Libro buscarLibro(string titulo)
        {
            foreach (var libro in libros)
            {
                if (libro.titulo.Equals(titulo))
                    return libro;
            }
            return null;
        }

        // 🔥 Nueva función: prestar libro desde la biblioteca
        public string prestarLibro(LectoresRegistrados registro, string titulo, int dni)
        {
            // Buscar lector
            Lector lector = registro.buscarLector(dni);
            if (lector == null)
                return "LECTOR INEXISTENTE";

            // Buscar libro
            Libro libro = buscarLibro(titulo);
            if (libro == null)
                return "LIBRO INEXISTENTE";

            if (libro.prestado)
                return "El libro ya está prestado";

            if (lector.prestamosActivos >= 3)
                return "TOPE DE PRÉSTAMO ALCANZADO";

            // Marcar como prestado
            libro.prestado = true;

            // Agregar a la lista de préstamos del lector
            lector.prestamos.Add(libro);

            // Aumentar el contador
            lector.prestamosActivos++;

            // Eliminar de la biblioteca
            eliminarLibro(titulo);

            return "PRÉSTAMO EXITOSO";
        }
    }


    // Clase Lectores
    public class Lector
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int dni { get; set; }
        public int prestamosActivos { get; set; }
        public List<Libro> prestamos { get; set; } // Lista de libros prestados

        public Lector(string nombre, string apellido, int dni, int prestamosActivos)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
            this.prestamosActivos = prestamosActivos;
            this.prestamos = new List<Libro>(); // Inicializamos lista vacía
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

        public string registrarLector(string nombre, string apellido, int dni, int prestamosActivos)
        {
            foreach (var lector in lectores)
            {
                if (lector.dni == dni)
                    return "El lector ya está registrado.";
            }

            lectores.Add(new Lector(nombre, apellido, dni, prestamosActivos));
            return "Lector registrado correctamente.";
        }

        //Función para buscar si el lector existe
        public Lector buscarLector(int dni)
        {
            foreach (var lector in lectores)
            {
                if (lector.dni == dni)
                    return lector;
            }
            
            return null;
        }

        // Función para devolver un libro prestado
        public string devolverLibro(Biblioteca biblioteca, string titulo, int dni)
        {
            Lector lector = buscarLector(dni);
            if (lector == null)
                return "LECTOR INEXISTENTE";

            // Buscar libro en la lista de préstamos del lector
            Libro libro = lector.prestamos.Find(l => l.titulo == titulo);
            if (libro == null)
                return "Ese libro no está en préstamo por este lector";

            // Marcar como disponible
            libro.prestado = false;

            // Eliminar de la lista de préstamos del lector
            lector.prestamos.Remove(libro);

            // Disminuir el contador (solo si es > 0)
            if (lector.prestamosActivos > 0)
                lector.prestamosActivos--;

            // Devolver a la biblioteca (usando la firma existente)
            biblioteca.agregarLibro(libro.titulo, libro.autor, libro.editorial);

            return "Devolución exitosa";
        }


    }



    // Pruebas a ver si el codigo funciona
    class Test
    {
        static void Main(string[] args)
        {
            // Se comienza creando la biblioteca
            Console.WriteLine("--- Se crea la biblioteca ---");
            Domicilio dom = new Domicilio("Calle Falsa", 123);
            Biblioteca biblioteca = new Biblioteca("Biblio29", dom);

            // Se agregan algunos libros 
            Console.WriteLine("--- Se suman libros a la biblioteca ---");
            biblioteca.agregarLibro("Libro1", "Autor1", "Editorial1");
            biblioteca.agregarLibro("Libro2", "Autor2", "Editorial2");
            biblioteca.agregarLibro("Libro3", "Autor3", "Editorial3");
            biblioteca.agregarLibro("Libro4", "Autor4", "Editorial4");
            biblioteca.agregarLibro("Libro5", "Autor5", "Editorial5");
            biblioteca.agregarLibro("Libro6", "Autor6", "Editorial6");

            Console.WriteLine("Libros en la biblioteca:");
            biblioteca.listarLibros();

            // Crear registro de lectores (todos con 0 préstamos activos al inicio, excepto Ana que se le van a cargar 3 prestamos)
            Console.WriteLine("--- Se crea un registro de lectores ---");
            LectoresRegistrados registro = new LectoresRegistrados();
            Console.WriteLine(registro.registrarLector("Juan", "Perez", 12345678, 0));
            Console.WriteLine(registro.registrarLector("Ana", "Gomez", 87654321, 0));
            Console.WriteLine(registro.registrarLector("Pepe", "Lopez", 22222222, 0));

            // Se le cargan de forma inicial 3 prestamos a Ana 
            Console.WriteLine("Se le agregan 3 prestamos a Ana");
            Console.WriteLine(biblioteca.prestarLibro(registro, "Libro2", 87654321)); 
            Console.WriteLine(biblioteca.prestarLibro(registro, "Libro3", 87654321)); 
            Console.WriteLine(biblioteca.prestarLibro(registro, "Libro4", 87654321)); 

            Console.WriteLine("Estado biblioteca después de pre-préstamos:");
            biblioteca.listarLibros();

            // Pruebas de prestamos 
            Console.WriteLine("--- Tests PrestarLibro ---");

            // 1) Préstamo exitoso
            Console.WriteLine("--- Préstamo exitoso (Juan pide Libro1) ---");
            Console.WriteLine(biblioteca.prestarLibro(registro, "Libro1", 12345678));

            // 2) Intentar prestar libro que está marcado como Prestado (buscamos un libro y cambiamos su propiedad "Prestado" a  = true)
            Console.WriteLine("--- Intentar prestar libro ya prestado --- ");
            var libroExistente = biblioteca.buscarLibro("Libro5");
            if (libroExistente != null) libroExistente.prestado = true; // lo marcamos como prestado
            Console.WriteLine(biblioteca.prestarLibro(registro, "Libro5", 12345678));
           


            // 3) Libro inexistente
            Console.WriteLine("--- Libro inexistente --- " + biblioteca.prestarLibro(registro, "LibroNoExiste", 12345678));
          


            // 4) Lector inexistente
            Console.WriteLine("--- Lector inexistente --- " + biblioteca.prestarLibro(registro, "Libro5", 99999999));

            // 5) Lector alcanzó máximo de préstamos activos (Siguiendo lo cargado anteriormente, Ana ya tiene 3)
            Console.WriteLine("--- Lector alcanzó máximo de préstamos activos ---");
            Console.WriteLine(biblioteca.prestarLibro(registro, "Libro5", 87654321));

            // ---- Tests DevolverLibro: cubrir todos los resultados posibles ----
            Console.WriteLine("=== Tests DevolverLibro ===");

            // 1) Devolución exitosa (Ana devuelve uno de sus libros)
            Console.WriteLine("--- Devolución exitosa (Ana devuelve Libro2) ---");
            Console.WriteLine(registro.devolverLibro(biblioteca, "Libro2", 87654321)); 

            // 2) Intentar devolver mismo libro otra vez (ya devuelto)
            Console.WriteLine(" --- Intentar devolver mismo libro otra vez (ya devuelto) ---");
            Console.WriteLine(registro.devolverLibro(biblioteca, "Libro2", 87654321)); 

            // 3) Lector inexistente en devolución
            Console.WriteLine("--- Lector inexistente intenta realizar una devolución ---");
            Console.WriteLine(registro.devolverLibro(biblioteca, "Libro6", 99999999)); 

            // 4) Devolver libro que el lector no tiene (Pepe intenta devolver Libro3)
            Console.WriteLine(" Devolver libro que el lector no tiene (Pepe intenta devolver Libro3):");
            Console.WriteLine(registro.devolverLibro(biblioteca, "Libro3", 22222222));


            // ---- Tests de AgregarLibro / EliminarLibro ----
            Console.WriteLine("--- Tests AgregarLibro / EliminarLibro ---");

            // Intentar agregar duplicado (Libro5 ya existe)
            Console.WriteLine("--- Agregar libro con título existente (Libro5) ---");
            bool agregado = biblioteca.agregarLibro("Libro5", "Autor5", "Editorial5");
            Console.WriteLine(agregado ? "Agregado" : "No agregado (ya existe)");

            // Eliminar libro existente (Libro6)
            Console.WriteLine("--- Eliminar libro existente (Libro6) ---");
            Console.WriteLine(biblioteca.eliminarLibro("Libro6") ? "Eliminado" : "No eliminado");

            // Eliminar libro inexistente
            Console.WriteLine("--- Eliminar libro inexistente (NoExiste) ---");
            Console.WriteLine(biblioteca.eliminarLibro("NoExiste") ? "Eliminado" : "No eliminado");

            // ---- Estado final ----
            Console.WriteLine("--- Estado final de la biblioteca ---");
            biblioteca.listarLibros();

            Console.WriteLine("--- Estado de lectores (PrestamosActivos y títulos) ---");
            var juan = registro.buscarLector(12345678);
            var ana = registro.buscarLector(87654321);
            var pepe = registro.buscarLector(22222222);

            Console.WriteLine($"Juan: PrestamosActivos={juan.prestamosActivos}, Prestamos=[{string.Join(", ", juan.prestamos.ConvertAll(l => l.titulo))}]");
            Console.WriteLine($"Ana: PrestamosActivos={ana.prestamosActivos}, Prestamos=[{string.Join(", ", ana.prestamos.ConvertAll(l => l.titulo))}]");
            Console.WriteLine($"Pepe: PrestamosActivos={pepe.prestamosActivos}, Prestamos=[{string.Join(", ", pepe.prestamos.ConvertAll(l => l.titulo))}]");

            Console.ReadLine();
        }
    }


}
