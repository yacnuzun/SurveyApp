// src/types/AnswerTemplate.ts

export interface TemplateOption {
  id: number;
  text: string;
  order: number;
}

export interface AnswerTemplate {
  id: number;
  name: string;
  optionCount: number;
  options: TemplateOption[];
}

export interface AnswerTemplateCreateDto {
  name: string;
  options: { text: string; order: number }[];
}

export interface AnswerTemplateUpdateDto extends AnswerTemplateCreateDto {
  id: number;
}