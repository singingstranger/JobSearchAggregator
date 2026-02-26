/*import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'*/
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

    useEffect(() => {
        fetch("http://localhost:5086/api/jobs?keyword=developer&page=1&pageSize=5")
            .then(res => res.json())
            .then(data => setJobs(data))
            .catch(err => console.log(err))
    }, []);
    return (
        <div>
            <h1> Job Search </h1>
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

/*
function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}*/

export default App
