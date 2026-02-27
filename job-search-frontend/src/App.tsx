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
            const response = await fetch(`http://localhost:5086/api/jobs?keyword=${keyword}&location=${location}&page=1&pageSize=${daysBack}`
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
        <div className="text-center mb-12 relative pt-24 md:pt-32">
            <div className="absolute inset-0 bg-gradient-to-r from-green-50 via-white to-purple-50 rounded-3xl"></div>
            <div className="relative z-10 max-w-3xl mx-auto">
                <div className="mb-6">
                    <span className="text-4xl md:text-5xl font-black tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-emerald-500 via-blue-200 to-purple-600">
                        JJA
                    </span>
                </div>
                <h1 className="text-5xl md:text-6xl font-extrabold text-gray-900 mb-4">
                    Find Your Next Opportunity
                </h1>
                <p className="text-lg md:text-xl text-gray-700 mb-6">
                    Browse thousands of jobs, filter by keyword and location, and discover your next role.
                </p>
                <div className="mt-6 h-1 w-24 bg-blue-500 rounded-full mx-auto"></div>
                
                <form
                    onSubmit={searchJobs}
                    className="animate-fadeIn bg-white p-6 rounded-2xl shadow-xl mb-10 flex flex-col md:flex-row gap-4 md:items-end md:justify-between"
                >
                    <div className="flex flex-col flex-1">
                        <label className="text-gray-700 font-medium mb-1">Keyword</label>
                        <input
                            type="text"
                            placeholder="Developer, Dentist..."
                            value={keyword}
                            onChange={(e) => setKeyword(e.target.value)}
                            className="border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-purple-400 transition"
                        />
                    </div>

                    <div className="flex flex-col flex-1">
                        <label className="text-gray-700 font-medium mb-1">Location</label>
                        <input
                            type="text"
                            placeholder="i.e. London"
                            value={location}
                            onChange={(e) => setLocation(e.target.value)}
                            className="border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-purple-400 transition"
                        />
                    </div>

                    <div className="flex flex-col w-32">
                        <label className="text-gray-700 font-medium mb-1">Days back</label>
                        <input
                            type="number"
                            value={daysBack}
                            min={1}
                            onChange={(e) => setDaysBack(Number(e.target.value))}
                            className="border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 focus:ring-purple-400 transition"
                        />
                    </div>

                    <button
                        type="submit"
                        className="bg-gradient-to-r from-purple-500 to-purple-700 text-white px-8 py-3 rounded-xl font-semibold hover:scale-105 hover:shadow-lg transition"
                    >
                        Search
                    </button>
                </form>
                {isLoading && (
                    <div className="flex flex-col items-center my-10">
                        <div className="h-12 w-12 border-4 border-purple-400 border-t-transparent rounded-full animate-spin"></div>
                        <p className="mt-4 text-gray-500">Searching jobs...</p>
                    </div>
                )}
                {error && (
                    <div className="animate-fadeIn mb-6 rounded-xl border border-red-200 bg-red-50 p-4 shadow-sm">
                        <div className="flex items-start gap-3">
                            <div className="text-red-500 mt-1">
                                ⚠️
                            </div>
                            <div>
                                <h4 className="font-semibold text-red-700">
                                    Something went wrong
                                </h4>
                                <p className="text-sm text-red-600 mt-1">
                                    {error}
                                </p>
                            </div>
                        </div>
                    </div>
                )}
                {!isLoading && jobs.length === 0 && (
                    <div className="text-center text-purple-400 mt-12">
                        No jobs found. Try adjusting your search.
                    </div>
                )}
                {jobs.map((job, index) => (
                    <a
                        key={job.originalURL}
                        href={job.originalURL}
                        target="_blank"
                        rel="noopener noreferrer"
                        style={{ animationDelay: `${index * 80}ms` }}
                        className="relative animate-fadeIn block bg-white rounded-2xl p-6 shadow-md hover:shadow-xl hover:-translate-y-1 transition duration-300 border border-gray-100 mb-6"
                    >
                        {job.isRemote && (
                            <span className="absolute top-4 right-4 bg-emerald-100 text-emerald-700 text-xs font-semibold px-3 py-1 rounded-full border border-emerald-200">
                                Remote
                            </span>
                        )}
                        <div className="flex flex-col md:flex-row justify-between gap-6">

                            {/* LEFT SIDE */}
                            <div className="flex-1 text-left">
                                <h3 className="text-xl font-bold mb-2 bg-clip-text text-transparent bg-gradient-to-r from-purple-500 to-purple-700">
                                    {job.title}
                                </h3>

                                <p className="text-gray-700 font-medium">
                                    {job.company}
                                </p>

                                <p className="text-gray-500 text-sm">
                                    {job.location}
                                </p>

                                <p className="text-gray-400 text-sm mt-2">
                                    Posted: {job.postedDate
                                    ? new Date(job.postedDate).toLocaleDateString()
                                    : "Date not available"}
                                </p>
                            </div>

                            {/* RIGHT SIDE */}
                            <div className="flex flex-wrap gap-2 items-start md:items-center">

                                <span className="bg-purple-100 text-purple-700 text-xs font-semibold px-3 py-1 rounded-full">
                                    {job.source}
                                </span>

                                <span className="bg-purple-50 text-purple-400 text-xs font-semibold px-3 py-1 rounded-full border border-purple-700">
                                    {job.minSalary != null && job.maxSalary != null
                                        ? `£${job.minSalary} - £${job.maxSalary}` 
                                        : "Salary not provided"} 
                                </span>

                            </div>
                        </div>
                    </a>
                ))}
            </div>
        </div>
    );
}

export default App
