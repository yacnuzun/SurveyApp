import { create } from 'zustand';
import { type SurveyReport } from '../types/Report';
import { fetchSurveyReport } from '../services/reportService';

interface ReportState {
    report: SurveyReport | null;
    isLoading: boolean;
    getReport: (id: number) => Promise<void>;
}

export const useReportStore = create<ReportState>((set) => ({
    report: null,
    isLoading: false,
    getReport: async (id) => {
        set({ isLoading: true });
        try {
            const data = await fetchSurveyReport(id);
            set({ report: data, isLoading: false });
        } catch (error) {
            set({ isLoading: false });
            console.error("Rapor çekilemedi", error);
        }
    }
}));