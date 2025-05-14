import axios from "axios";

class CountryService {
  async getCountries(): Promise<{ name: string; flag: string }[]> {
    try {
      const res = await axios.get("https://countriesnow.space/api/v0.1/countries/flag/images");

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new CountryService();
