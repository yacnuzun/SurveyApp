export interface QuestionReport {
    questionText: string;
    labels: string[];
    data: number[];
}

export interface SurveyReport {
    surveyId: number;
    surveyTitle: string | null;
    totalParticipants: number;
    questionResults: QuestionReport[];
}