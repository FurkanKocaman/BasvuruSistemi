// İş ilanları için dummy veri
export interface Job {
  id: number;
  title: string;
  company: string;
  location: string;
  description: string;
  requirements: string[];
  salary: string;
  postDate: string;
  department: string;
}

export const jobData: Job[] = [
  {
    id: 1,
    title: 'Frontend Geliştirici',
    company: 'Tech Solutions A.Ş.',
    location: 'İstanbul, Türkiye',
    description: 'Modern web uygulamaları geliştirmek için Vue.js ve React deneyimi olan bir frontend geliştirici arıyoruz.',
    requirements: ['Vue.js', 'React', 'TypeScript', 'Tailwind CSS', 'Git'],
    salary: '25,000 - 35,000 TL',
    postDate: '2025-04-15',
    department: 'Yazılım Geliştirme'
  },
  {
    id: 2,
    title: 'Backend Geliştirici',
    company: 'Digital Çözümler Ltd.',
    location: 'Ankara, Türkiye',
    description: 'Node.js ve Express kullanarak ölçeklenebilir API\'ler geliştirecek bir backend geliştirici arıyoruz.',
    requirements: ['Node.js', 'Express', 'MongoDB', 'REST API', 'Mikroservisler'],
    salary: '28,000 - 40,000 TL',
    postDate: '2025-04-20',
    department: 'Yazılım Geliştirme'
  },
  {
    id: 3,
    title: 'UX/UI Tasarımcı',
    company: 'Kreatif Ajans',
    location: 'İzmir, Türkiye',
    description: 'Kullanıcı deneyimi tasarımı konusunda deneyimli, yaratıcı bir UX/UI tasarımcı arıyoruz.',
    requirements: ['Figma', 'Adobe XD', 'UI Tasarım', 'UX Araştırma', 'Prototipleme'],
    salary: '22,000 - 30,000 TL',
    postDate: '2025-04-25',
    department: 'Tasarım'
  },
  {
    id: 4,
    title: 'Veri Bilimci',
    company: 'Analitik Teknolojiler',
    location: 'İstanbul, Türkiye',
    description: 'Büyük veri kümeleri üzerinde analiz yapabilecek, makine öğrenimi modellerini geliştirebilecek bir veri bilimci arıyoruz.',
    requirements: ['Python', 'R', 'SQL', 'Machine Learning', 'TensorFlow', 'Pandas'],
    salary: '30,000 - 45,000 TL',
    postDate: '2025-05-01',
    department: 'Veri Bilimi'
  },
  {
    id: 5,
    title: 'DevOps Mühendisi',
    company: 'Bulut Sistemleri A.Ş.',
    location: 'Ankara, Türkiye',
    description: 'CI/CD süreçlerini otomatikleştirecek, bulut altyapısını yönetecek bir DevOps mühendisi arıyoruz.',
    requirements: ['Docker', 'Kubernetes', 'AWS', 'CI/CD', 'Terraform', 'Linux'],
    salary: '32,000 - 48,000 TL',
    postDate: '2025-05-05',
    department: 'Altyapı'
  }
];