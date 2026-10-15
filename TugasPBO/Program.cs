using System;
using System.Collections.Generic;

namespace SistemKaryawan
{
    public class Karyawan
    {
        public string Nama { get; set; }
        public decimal Gaji { get; set; }

        public Karyawan(string nama, decimal gaji)
        {
            Nama = nama;
            Gaji = gaji;
        }

        public virtual void Kerja()
        {
            Console.WriteLine($"{Nama} sedang bekerja.");
        }

        public virtual void InfoKaryawan()
        {
            Console.WriteLine($"Nama: {Nama} | Gaji: Rp{Gaji:N0}");
        }
    }

    public class Tetap : Karyawan
    {
        public decimal Tunjangan { get; set; }

        public Tetap(string nama, decimal gaji, decimal tunjangan) : base(nama, gaji)
        {
            Tunjangan = tunjangan;
        }

        public decimal HitungGajiTotal()
        {
            return Gaji + Tunjangan;
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (karyawan tetap) bekerja sesuai jam kantor.");
        }

        public override void InfoKaryawan()
        {
            base.InfoKaryawan();
            Console.WriteLine($"   Tunjangan: Rp{Tunjangan:N0} | Gaji Total: Rp{HitungGajiTotal():N0}");
        }
    }

    public class Kontrak : Karyawan
    {
        public TimeSpan Durasi { get; set; }

        public Kontrak(string nama, decimal gaji, TimeSpan durasi) : base(nama, gaji)
        {
            Durasi = durasi;
        }

        public void CekKontrak()
        {
            int hari = (int)Durasi.TotalDays;
            Console.WriteLine($"Durasi kontrak {Nama}: {hari} hari.");

            if (Durasi <= TimeSpan.Zero)
                Console.WriteLine("   Status: Kontrak tidak valid.");
            else if (hari < 180)
                Console.WriteLine("   Status: Kontrak jangka pendek.");
            else if (hari <= 365)
                Console.WriteLine("   Status: Kontrak jangka menengah.");
            else
                Console.WriteLine("   Status: Kontrak jangka panjang.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (kontrak) bekerja selama masa kontrak.");
        }

        public override void InfoKaryawan()
        {
            base.InfoKaryawan();
            Console.WriteLine($"   Durasi Kontrak: {(int)Durasi.TotalDays} hari");
        }
    }

    public class Manager : Tetap
    {
        public Manager(string nama, decimal gaji, decimal tunjangan) : base(nama, gaji, tunjangan) { }

        public void Memimpin()
        {
            Console.WriteLine($"{Nama} sedang memimpin rapat tim.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (manager) mengatur dan mengawasi pekerjaan tim.");
        }
    }

    public class Staff : Tetap
    {
        public Staff(string nama, decimal gaji, decimal tunjangan) : base(nama, gaji, tunjangan) { }

        public void KerjakanTugas()
        {
            Console.WriteLine($"{Nama} sedang mengerjakan tugas harian.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (staff) menyelesaikan tugas dari manager.");
        }
    }

    public class Magang : Kontrak
    {
        public Magang(string nama, decimal gaji, TimeSpan durasi) : base(nama, gaji, durasi) { }

        public void Belajar()
        {
            Console.WriteLine($"{Nama} sedang belajar dari mentor.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (magang) membantu pekerjaan sambil belajar.");
        }
    }

    public class Freelancer : Kontrak
    {
        public Freelancer(string nama, decimal gaji, TimeSpan durasi) : base(nama, gaji, durasi) { }

        public void AmbilProyek()
        {
            Console.WriteLine($"{Nama} mengambil proyek baru.");
        }

        public override void Kerja()
        {
            Console.WriteLine($"{Nama} (freelancer) mengerjakan proyek dari rumah.");
        }
    }

    public class Perusahaan
    {
        public string Nama { get; set; }
        private List<Karyawan> daftarKaryawan = new List<Karyawan>();

        public Perusahaan(string nama)
        {
            Nama = nama;
        }

        public void TambahKaryawan(Karyawan karyawan)
        {
            daftarKaryawan.Add(karyawan);
            Console.WriteLine($"{karyawan.Nama} ditambahkan ke {Nama}.");
        }

        public void DaftarKaryawan()
        {
            Console.WriteLine($"\n=== Daftar Karyawan {Nama} ===");
            int no = 1;
            foreach (Karyawan k in daftarKaryawan)
            {
                Console.Write($"{no++}. ");
                k.InfoKaryawan();
            }
        }

        public IReadOnlyList<Karyawan> Semua => daftarKaryawan;
    }

    class Program
    {
        static void Main(string[] args)
        {

            Perusahaan perusahaan = new Perusahaan("PT Maju Jaya");

            Manager manager = new Manager("Andi", 10000000m, 3000000m);
            Staff staff = new Staff("Siti", 6000000m, 1000000m);
            Magang magang = new Magang("Rina", 1500000m, TimeSpan.FromDays(90));
            Freelancer freelancer = new Freelancer("Dodi", 4000000m, TimeSpan.FromDays(400));

            perusahaan.TambahKaryawan(manager);
            perusahaan.TambahKaryawan(staff);
            perusahaan.TambahKaryawan(magang);
            perusahaan.TambahKaryawan(freelancer);

            perusahaan.DaftarKaryawan();

            Console.WriteLine("\n=== Demonstrasi Polymorphism ===");
            foreach (Karyawan k in perusahaan.Semua)
            {
                k.Kerja();
            }

            Console.WriteLine("\n=== Method Khusus ===");
            manager.Memimpin();
            staff.KerjakanTugas();
            magang.Belajar();
            freelancer.AmbilProyek();

            Console.WriteLine();
            magang.CekKontrak();
            freelancer.CekKontrak();
            Console.WriteLine($"Gaji total {manager.Nama}: Rp{manager.HitungGajiTotal():N0}");

            Console.ReadLine();
        }
    }
}