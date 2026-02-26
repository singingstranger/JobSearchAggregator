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
        <div>
            <h1> Job Search </h1>
            <form onSubmit={searchJobs} style={{ marginBottom: "20%" }}>
                
                <label style={{ marginLeft: "20px" }}>
                    Keyword: 
                    <input 
                        type="text"
                        placeholder="Developer, Dentist..."
                        value={keyword}
                        onChange={(e) => setKeyword(e.target.value)}
                        style={{ marginLeft: "5px" }}
                    />
                </label>
                <label style={{ marginLeft: "20px" }}>
                    Location:
                    <input 
                        type="text"
                        placeholder="i.e. London"
                        value={location}
                        onChange={(e) => setLocation(e.target.value)}
                        style={{ marginLeft: "5px" }}
                    />
                </label>
                
                <label style={{ marginLeft: "20px" }}>
                    Days back:
                    <input
                        type="number"
                        value={daysBack}
                        onChange={(e) => setDaysBack(Number(e.target.value))}
                        min={1}
                        style={{ marginLeft: "5px" }}
                    />
                </label>
                <p></p>
                <button type="submit" style={{marginLeft: "10px"}}>Search</button>

            </form>
            {isLoading && <div className="spinner"></div> }
            {error && <div className="error" style={{color: "red"}}>{error}</div>}
            {jobs.map(job =>(
                <div key={job.originalURL} style={{ border: "1px solid gray", margin: "10px", padding: "10px" }}>
                    <h3> 
                        <a href={job.originalURL} target={"_blank"}>{job.title}</a>
                    </h3>
                    <p>{job.company} --- {job.location}</p>
                    <p>{job.minSalary && job.maxSalary
                    ? `£${job.minSalary} - £${job.maxSalary}`
                        : "Salary not listed"}
                    </p>
                    <p>{job.isRemote ? "Remote" : "On-Site/Hybrid"} -- {job.source} </p>
                </div>
            ))}
        </div>
    );
}

export default App
