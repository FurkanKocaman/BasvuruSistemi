namespace BasvuruSistemi.Server.Domain.Enums;
public enum SkillLevel
{
    None = 0,
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4,
    Master = 5
}


public enum ProfileStatus
{
    Draft = 0,         // Henüz tamamlanmamış
    InReview = 1,      // Admin onayı bekliyor
    Completed = 2,     // Kullanıcı tarafından tamamlanmış
    Verified = 3,      // Yetkili tarafından onaylanmış
    Suspended = 4,     // Hatalı/uygunsuz içerik nedeniyle askıya alınmış
    Archived = 5       // Pasif durum (silinmedi ama gösterilmiyor)
}

public enum ApplicationStatus
{
    Pending = 0,
    InReview = 1,       // Onaylayıcı tarafından inceleniyor
    Approved = 2,
    Rejected = 3,
    Withdrawn = 4,      // Kullanıcı tarafından geri çekildi
    Expired = 5,        // İlan süresi geçti, otomatik reddedildi
    InterviewScheduled = 6, 
    OfferMade = 7,      // İş teklifi yapıldı
    Hired = 8,           // Süreç tamamlandı
    JobPostingClosed = 9,
}

public enum VerificationStatus
{
    NotSubmitted = 0,
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    NeedsCorrection = 4, // Belge eksik veya yanlış
    Expired = 5          // Süresi dolmuş belge
}

public enum FieldTypeEnum
{
    Textbox = 1,
    Textarea = 2,
    Checkbox = 3,
    RadioButton = 4,
    Dropdown = 5,
    File = 6,
    DatePicker = 7,
    Number = 8,
    Email = 9,
    PhoneNumber = 10,
    TCKN = 11,
    IBAN = 12,
    Image = 13, // Profil fotoğrafı, belge görseli
    URL = 14,
    EDevletVerifiedFile = 15, // e-Devlet doğrulamalı dosya
    YoksisAlesDocument = 16,  // YÖKSİS ALES Belgesi (Dosya olarak yüklenip doğrulanacak)
    YoksisAlesScore = 17,     // YÖKSİS ALES Puanı (Yıl seçimi ile YÖKSİS'ten çekilecek)
    YoksisKpssScore = 18,     // YÖKSİS KPSS Puanı (Yıl ve Puan Türü seçimi ile YÖKSİS'ten çekilecek)
}

// Bu enum, eğer FieldTypeEnum'da YoksisAlesScore, YoksisKpssScore gibi
// ayrıştırmalar yapmak yerine genel bir "YoksisDataField" tipi kullanıp
// bu enum ile detaylandırmak istersek kullanılabilir.
// Şimdiki FieldTypeEnum genişletmesiyle buna direkt ihtiyaç olmayabilir
// ama alternatif olarak değerlendirilebilir.
public enum YoksisDataTypeEnum
{
    AlesScore = 1,
    KpssScore = 2,
    Transcript = 3, // Örnek olarak eklendi
    GraduationCertificate = 4 // Örnek olarak eklendi
}
public enum VerificationSourceEnum
{
    None = 0,
    EDevlet = 1,
    Yoksis = 2
    // Gelecekte eklenebilecek diğer kaynaklar
}

public enum JobPostingStatus
{
    Draft = 1,       // Taslak, henüz yayınlanmamış
    Published = 2,   // Yayınlanmış ve aktif (başvuru alabilir)
    OnHold = 3,      // Geçici olarak durdurulmuş
    Closed = 4,      // Kapatılmış (manuel olarak veya pozisyon dolduğu için)
    Expired = 5,     // Son başvuru tarihi geçmiş
    Archived = 6     // Arşivlenmiş, kayıtlarda saklanıyor
}

public enum EmploymentType
{
    FullTime = 1,    // Tam Zamanlı
    PartTime = 2,    // Yarı Zamanlı
    Contract = 3,    // Sözleşmeli
    Internship = 4,  // Staj
    Temporary = 5,   // Geçici
    Freelance = 6,   // Serbest Çalışan
    ProjectBased = 7 // Proje Bazlı
}

public enum ExperienceLevel // Deneyim Seviyesi
{
    Student = 1,          // Öğrenci (Staj vb. için)
    EntryLevel = 2,       // Yeni Başlayan
    Junior = 3,           // 1-3 Yıl Deneyimli
    MidLevel = 4,         // 3-5 Yıl Deneyimli
    Senior = 5,           // 5+ Yıl Deneyimli
    Lead = 6,             // Takım Lideri / Kıdemli Uzman
    Principal = 7,        // Baş Uzman / Mimar
    Manager = 8,          // Yönetici
    Director = 9,         // Direktör
    Executive = 10        // Üst Düzey Yönetici
}

public enum NotificationType
{
    System = 0,
    ApplicationStatusChange = 1,
    NewMessage = 2,
    DocumentVerification = 3,
    InterviewInvite = 4,
    Reminder = 5
}

public enum DocumentType
{
    IdentityCard = 0,
    Diploma = 1,
    Transcript = 2,
    Certificate = 3,
    Resume = 4,
    CoverLetter = 5,
    Other = 99
}
