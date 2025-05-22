import { ClaimType } from "@/models/constants/claim-type";
import { useUserStore } from "@/stores/user";

export function useClaimChecker() {
  const userStore = useUserStore();

  /**
   * Kullanıcının belirtilen claim(ler)e sahip olup olmadığını kontrol eder.
   * @param claims - Kontrol edilecek claim veya claim listesi
   * @returns Sahip ise true, değilse false
   */
  function hasClaim(claims: ClaimType | ClaimType[]): boolean {
    const userClaims = userStore.user?.claims || [];

    // Tek claim string olarak verilmişse diziye çevir
    const requiredClaims = Array.isArray(claims) ? claims : [claims];

    return requiredClaims.some((claim) => userClaims.includes(claim));
  }

  return { hasClaim };
}
