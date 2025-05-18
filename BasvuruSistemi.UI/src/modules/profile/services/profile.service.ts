import { Profile, MaritalStatus, SkillLevel } from '../models/profile.model';

// Profil servisi - şu an için statik veriler içeriyor
// Daha sonra backend bağlantısı eklenecek
export class ProfileService {
  // Kullanıcı profil bilgilerini getir
  static async getUserProfile(): Promise<Profile> {
    // Şu an için dummy veri dönüyoruz
    // Gerçek uygulamada bu veri API'den gelecek
    return {
      id: '1',
      firstName: 'Ahmet',
      lastName: 'Yılmaz',
      tcKimlikNo: '12345678910',
      maritalStatus: MaritalStatus.Married,
      profilePicture: 'https://randomuser.me/api/portraits/men/75.jpg',
      education: [
        {
          id: '1',
          schoolName: 'İstanbul Teknik Üniversitesi',
          degree: 'Lisans',
          fieldOfStudy: 'Bilgisayar Mühendisliği',
          startDate: '2015-09-01',
          endDate: '2019-06-30',
          isCurrentlyStudying: false,
          description: 'Bilgisayar Mühendisliği bölümünden mezun oldum.'
        },
        {
          id: '2',
          schoolName: 'Boğaziçi Üniversitesi',
          degree: 'Yüksek Lisans',
          fieldOfStudy: 'Yapay Zeka',
          startDate: '2019-09-01',
          endDate: null,
          isCurrentlyStudying: true,
          description: 'Yapay Zeka alanında yüksek lisans eğitimime devam ediyorum.'
        }
      ],
      certificates: [
        {
          id: '1',
          name: 'AWS Certified Solutions Architect',
          issuingOrganization: 'Amazon Web Services',
          issueDate: '2020-05-15',
          expirationDate: '2023-05-15',
          credentialId: 'AWS-123456',
          credentialUrl: 'https://aws.amazon.com/certification/'
        },
        {
          id: '2',
          name: 'Microsoft Certified: Azure Developer Associate',
          issuingOrganization: 'Microsoft',
          issueDate: '2021-03-10',
          expirationDate: null,
          credentialId: 'MS-789012',
          credentialUrl: 'https://learn.microsoft.com/certifications/'
        }
      ],
      workExperience: [
        {
          id: '1',
          title: 'Frontend Geliştirici',
          companyName: 'Teknoloji A.Ş.',
          location: 'İstanbul',
          startDate: '2019-07-15',
          endDate: '2022-01-31',
          isCurrentlyWorking: false,
          description: 'Vue.js ve React kullanarak web uygulamaları geliştirdim.'
        },
        {
          id: '2',
          title: 'Kıdemli Frontend Geliştirici',
          companyName: 'Yazılım Ltd. Şti.',
          location: 'İstanbul',
          startDate: '2022-02-01',
          endDate: null,
          isCurrentlyWorking: true,
          description: 'Vue 3 ve TypeScript ile kurumsal web uygulamaları geliştiriyorum.'
        }
      ],
      skills: [
        {
          id: '1',
          name: 'Vue.js',
          level: SkillLevel.Expert
        },
        {
          id: '2',
          name: 'TypeScript',
          level: SkillLevel.Advanced
        },
        {
          id: '3',
          name: 'Tailwind CSS',
          level: SkillLevel.Advanced
        },
        {
          id: '4',
          name: 'Node.js',
          level: SkillLevel.Intermediate
        }
      ]
    };
  }

  // Profil bilgilerini güncelle
  static async updateProfile(profile: Profile): Promise<boolean> {
    // Şu an için sadece başarılı olduğunu varsayıyoruz
    // Gerçek uygulamada API'ye gönderilecek
    console.log('Profil güncellendi:', profile);
    return true;
  }
}
