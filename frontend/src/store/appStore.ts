// src/store/appStore.ts — GÜNCELLENDİ

import { configureStore } from '@reduxjs/toolkit';
import authReducer from './authSlice';
import surveyReducer from './surveySlice';
import participationReducer from './participationSlice';
import answerTemplateReducer from './answerTemplateSlice';
import questionReducer from './questionSlice';
import userReducer from './userSlice';

export const store = configureStore({
  reducer: {
    auth: authReducer,
    survey: surveyReducer,
    participation: participationReducer,
    answerTemplate: answerTemplateReducer,
    question: questionReducer,
    user: userReducer, 
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;