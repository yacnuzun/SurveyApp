import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useReportStore } from '../../store/useReportStore';
import { Pie } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js';
import './ReportPage.css';

ChartJS.register(ArcElement, Tooltip, Legend);

const ReportPage = () => {
    const { id } = useParams<{ id: string }>();
    const { report, isLoading, getReport } = useReportStore();

    useEffect(() => {
        if (id) getReport(Number(id));
    }, [id, getReport]);

    if (isLoading) return <p>Yükleniyor...</p>;
    if (!report) return <p>Veri bulunamadı.</p>;

    return (
        <div className="report-container">
            <h1>Anket Raporu: {report.surveyId}</h1>
            <p>Toplam Katılım: {report.totalParticipants}</p>
            
            <div className="charts-grid">
                {report.questionResults.map((q, index) => (
                    <div key={index} className="chart-item" style={{ width: '400px', margin: '20px' }}>
                        <h3>{q.questionText}</h3>
                        <Pie data={{
                            labels: q.labels,
                            datasets: [{
                                data: q.data,
                                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0'],
                            }]
                        }} />
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ReportPage;