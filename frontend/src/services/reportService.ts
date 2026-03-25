import axios from 'axios';
import { type SurveyReport } from '../types/Report';

const API_BASE_URL = 'https://localhost:7025/api/reports'; // Kendi portunla güncelle

export const fetchSurveyReport = async (surveyId: number): Promise<SurveyReport> => {
    const token = localStorage.getItem('token');
    const response = await axios.get<SurveyReport>(`${API_BASE_URL}/${surveyId}`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    return response.data;
};