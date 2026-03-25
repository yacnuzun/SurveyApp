// src/types/Survey.ts — GÜNCELLENDİ
import type { TemplateOption } from './AnswerTemplate';


export const QuestionType = {
  Single: 0,
  Multi: 1,
  Text: 2,
} as const;
export type QuestionType = (typeof QuestionType)[keyof typeof QuestionType];

export interface Question {
  id: number;
  text: string;
  type: QuestionType;
  answerTemplateId?: number;
  answerTemplateName?: string;
  options: TemplateOption[];
}

export interface Survey {
  id: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  assignedUserIds: number[];   
  questions: Question[];       
}
export interface SurveyDetail extends Survey {
  questions: Question[];
}

// --- Create / Update DTOs ---

export interface QuestionCreateDto {
  text: string;
  type: QuestionType;
  answerTemplateId?: number;
}

export interface QuestionUpdateDto extends QuestionCreateDto {
  id: number;
}

export interface SurveyCreateDto {
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  questionIds: number[];
  assignedUserIds: number[];
}

export interface SurveyUpdateDto extends SurveyCreateDto {
  id: number;
  isActive: boolean;
}

// --- Participation (anket cevaplama) ---

export interface AnswerForCreateDto {
  questionId: number;
  optionId: number | null;      // TemplateOption.id — Single/Multi için
  textAnswer: string | null;    // Text tipi için
}

export interface ParticipationForCreateDto {
  surveyId: number;
  answers: AnswerForCreateDto[];
}