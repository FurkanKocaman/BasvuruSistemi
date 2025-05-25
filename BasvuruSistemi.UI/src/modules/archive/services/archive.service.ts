import { 
  ArchiveEntry, 
  UserArchiveData, 
  ApplicationArchiveData, 
  JobListingArchiveData 
} from '../models/archive.model';

// Dummy data for archive entries
const archiveEntries: ArchiveEntry[] = [
  {
    id: 1,
    year: 2023,
    name: '2023 Yılı Arşivi',
    description: '2023 yılına ait tüm başvuru ve ilan kayıtları',
    createdAt: '2023-12-31T23:59:59',
    status: 'ready',
    fileSize: '24.5 MB'
  },
  {
    id: 2,
    year: 2024,
    name: '2024 Yılı Arşivi',
    description: '2024 yılına ait tüm başvuru ve ilan kayıtları',
    createdAt: '2024-05-15T10:30:00',
    status: 'processing',
  }
];

// Dummy data for user archives
const userArchiveData: Record<number, UserArchiveData[]> = {
  2023: [
    {
      id: 1,
      fullName: 'Ahmet Yılmaz',
      tcKimlikNo: '12345678901',
      email: 'ahmet.yilmaz@example.com',
      phone: '05321234567',
      registrationDate: '2023-01-15T09:30:00',
      lastLoginDate: '2023-12-28T14:22:15'
    },
    {
      id: 2,
      fullName: 'Ayşe Demir',
      tcKimlikNo: '98765432109',
      email: 'ayse.demir@example.com',
      phone: '05331234567',
      registrationDate: '2023-02-20T11:45:00',
      lastLoginDate: '2023-12-30T16:10:45'
    },
    {
      id: 3,
      fullName: 'Mehmet Kaya',
      tcKimlikNo: '45678901234',
      email: 'mehmet.kaya@example.com',
      phone: '05341234567',
      registrationDate: '2023-03-10T14:20:00',
      lastLoginDate: '2023-12-29T10:05:30'
    }
  ],
  2024: [
    {
      id: 4,
      fullName: 'Zeynep Şahin',
      tcKimlikNo: '56789012345',
      email: 'zeynep.sahin@example.com',
      phone: '05351234567',
      registrationDate: '2024-01-05T10:15:00',
      lastLoginDate: '2024-05-20T09:30:45'
    },
    {
      id: 5,
      fullName: 'Mustafa Öztürk',
      tcKimlikNo: '67890123456',
      email: 'mustafa.ozturk@example.com',
      phone: '05361234567',
      registrationDate: '2024-01-18T13:40:00',
      lastLoginDate: '2024-05-19T11:25:10'
    }
  ]
};

// Dummy data for application archives
const applicationArchiveData: Record<number, ApplicationArchiveData[]> = {
  2023: [
    {
      id: 1,
      jobTitle: 'Yazılım Geliştirici',
      applicantName: 'Ahmet Yılmaz',
      applicationDate: '2023-03-15T10:30:00',
      status: 'Kabul Edildi',
      score: 85
    },
    {
      id: 2,
      jobTitle: 'Veri Analisti',
      applicantName: 'Ayşe Demir',
      applicationDate: '2023-04-20T14:45:00',
      status: 'Reddedildi',
      score: 65
    },
    {
      id: 3,
      jobTitle: 'Sistem Yöneticisi',
      applicantName: 'Mehmet Kaya',
      applicationDate: '2023-06-10T09:15:00',
      status: 'Değerlendirmede',
      score: 78
    }
  ],
  2024: [
    {
      id: 4,
      jobTitle: 'UI/UX Tasarımcısı',
      applicantName: 'Zeynep Şahin',
      applicationDate: '2024-02-05T11:20:00',
      status: 'Kabul Edildi',
      score: 92
    },
    {
      id: 5,
      jobTitle: 'Proje Yöneticisi',
      applicantName: 'Mustafa Öztürk',
      applicationDate: '2024-03-18T15:10:00',
      status: 'Değerlendirmede',
      score: 80
    }
  ]
};

// Dummy data for job listing archives
const jobListingArchiveData: Record<number, JobListingArchiveData[]> = {
  2023: [
    {
      id: 1,
      title: 'Yazılım Geliştirici',
      department: 'Bilgi Teknolojileri',
      publishDate: '2023-02-01T09:00:00',
      endDate: '2023-03-01T18:00:00',
      applicationsCount: 45,
      status: 'Kapalı'
    },
    {
      id: 2,
      title: 'Veri Analisti',
      department: 'Veri Bilimi',
      publishDate: '2023-03-15T09:00:00',
      endDate: '2023-04-15T18:00:00',
      applicationsCount: 32,
      status: 'Kapalı'
    },
    {
      id: 3,
      title: 'Sistem Yöneticisi',
      department: 'Bilgi Teknolojileri',
      publishDate: '2023-05-10T09:00:00',
      endDate: '2023-06-10T18:00:00',
      applicationsCount: 28,
      status: 'Kapalı'
    }
  ],
  2024: [
    {
      id: 4,
      title: 'UI/UX Tasarımcısı',
      department: 'Tasarım',
      publishDate: '2024-01-05T09:00:00',
      endDate: '2024-02-05T18:00:00',
      applicationsCount: 38,
      status: 'Kapalı'
    },
    {
      id: 5,
      title: 'Proje Yöneticisi',
      department: 'Proje Yönetimi',
      publishDate: '2024-02-15T09:00:00',
      endDate: '2024-03-15T18:00:00',
      applicationsCount: 25,
      status: 'Kapalı'
    },
    {
      id: 6,
      title: 'Mobil Uygulama Geliştirici',
      department: 'Bilgi Teknolojileri',
      publishDate: '2024-04-01T09:00:00',
      endDate: '2024-05-01T18:00:00',
      applicationsCount: 42,
      status: 'Açık'
    }
  ]
};

export const archiveService = {
  // Get all archive entries
  getArchiveEntries(): Promise<ArchiveEntry[]> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(archiveEntries);
      }, 500);
    });
  },

  // Get archive entry by year
  getArchiveEntryByYear(year: number): Promise<ArchiveEntry | undefined> {
    return new Promise((resolve) => {
      setTimeout(() => {
        const entry = archiveEntries.find(entry => entry.year === year);
        resolve(entry);
      }, 300);
    });
  },

  // Get user data by year
  getUserDataByYear(year: number): Promise<UserArchiveData[]> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(userArchiveData[year] || []);
      }, 500);
    });
  },

  // Get application data by year
  getApplicationDataByYear(year: number): Promise<ApplicationArchiveData[]> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(applicationArchiveData[year] || []);
      }, 500);
    });
  },

  // Get job listing data by year
  getJobListingDataByYear(year: number): Promise<JobListingArchiveData[]> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(jobListingArchiveData[year] || []);
      }, 500);
    });
  },

  // Download archive for a specific year
  downloadArchive(year: number): Promise<boolean> {
    return new Promise((resolve) => {
      setTimeout(() => {
        console.log(`Downloading archive for year ${year}`);
        resolve(true);
      }, 1500);
    });
  }
};
