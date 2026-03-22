export interface Survey {
  id: string;
  title: string;
  description: string;
  createdAt: string;
}

export const QuestionType = {
  Single: 0,
  Multi: 1,
  Text: 2
} as const;

export type QuestionType = typeof QuestionType[keyof typeof QuestionType];

export interface QuestionOption {
  id: number;
  text: string;
}

export interface Question {
  id: number;
  text: string;
  type: QuestionType;
  options: QuestionOption[] | null;
}

export interface SurveyDetail extends Survey {
  questions: Question[];
}

export interface OptionCreateDto {
  text: string;
  order: number;
}

export interface QuestionCreateDto {
  text: string;
  type: number; // 0: Single, 1: Multi, 2: Text
  order: number;
  options: OptionCreateDto[];
}

export interface SurveyCreateDto {
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  questions: QuestionCreateDto[];
}