import { useEffect, useState } from "react";
import './App.css'

type Job ={
    title: string,
    company: string,
    location: string,
    postedDate: string,
    source:string,
    originalURL: string,
    minSalary?: number,
    maxSalary?: number,
    jobType?: string,
    isRemote: boolean
}
function App(){
    const [jobs, setJobs] = useState<Job[]>([]);
    
    const [keyword, setKeyword] = useState("");
    const [location, setLocation] = useState("");
    const [daysBack, setDaysBack] = useState(3);

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const searchJobs = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        setIsLoading(true);
        setError(null);
        try{
            const response = await fetch(`http://localhost:5086/api/jobs?keyword=${keyword}&location=${location}&page=1&pageSize=5`
            );
            if(!response.ok)
            {
                throw new Error("Something went wrong, please try again later!");
            }
            const data = await response.json();
            setJobs(data);
        } catch(error: any){
            setError(error.message);
        } finally{
            setIsLoading(false);
        }
    };
    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="max-w-4xl mx-auto bg-white rounded-xl shadow-lg p-8">
                <h1 className="text-3xl font-bold text-gray-800 mb-6"> Job Search </h1>
                <form onSubmit={searchJobs} className="flex flex-wrap gap-4 mb-6">
                    
                    <label>
                        Keyword: 
                        <input 
                            type="text"
                            placeholder="Developer, Dentist..."
                            value={keyword}
                            onChange={(e) => setKeyword(e.target.value)}
                            className="border rounded-lg px-4 py-2 w-48 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </label>
                    <label style={{ marginLeft: "20px" }}>
                        Location:
                        <input 
                            type="text"
                            placeholder="i.e. London"
                            value={location}
                            onChange={(e) => setLocation(e.target.value)}
                            className="border rounded-lg px-4 py-2 w-48 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </label>
                    
                    <label style={{ marginLeft: "20px" }}>
                        Days back:
                        <input
                            type="number"
                            value={daysBack}
                            onChange={(e) => setDaysBack(Number(e.target.value))}
                            min={1}
                            className="border rounded-lg px-4 py-2 w-32 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </label>
                    <p></p>
                    <button 
                        type="submit" 
                        className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
                    >
                        Search
                    </button>
    
                </form>
                {isLoading && (
                    <div className="flex justify-center my-6">
                        <div className="h-10 w-10 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"></div>
                    </div>
                )}
                {error && <div className="error" style={{color: "red"}}>{error}</div>}
                {jobs.map(job =>(
                    <div
                        key={job.originalURL}
                        className="border rounded-lg p-4 shadow-sm hover:shadow-md transition mb-4"
                    >
                        <h3 className="text-lg font-semibold text-blue-600">
                            <a href={job.originalURL} target="_blank">
                                {job.title}
                            </a>
                        </h3>
                        <p className="text-gray-600">
                            {job.company} — {job.location}
                        </p>
                        <p className="text-gray-500 text-sm mt-1">
                            {job.minSalary && job.maxSalary
                                ? `£${job.minSalary} - £${job.maxSalary}`
                                : "Salary not listed"}
                        </p>
                        <p className="text-sm mt-2">
                            {job.isRemote ? "Remote" : "On-site/Hybrid"} • {job.source}
                        </p>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default App
