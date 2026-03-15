
<!DOCTYPE html>
<html lang="en">
<head>
</head>
<body>

<h1>SKF Product Assistant (Mini)</h1>

<p>
A lightweight AI-powered product assistant built using <b>C#, Azure Functions,
Microsoft Semantic Kernel, Azure OpenAI, and Redis</b>. The system answers natural‑language
questions about SKF bearing attributes using local JSON datasheets while maintaining
conversation context and capturing user feedback.
</p>

<div class="section box">
<h2>Architecture Overview</h2>

<pre>
User Request
     ↓
Azure Function HTTP Endpoint
     ↓
Intent Orchestrator (Semantic Kernel)
     ↓
 ┌───────────────┬────────────────┐
 │               │                │
Q&A Agent     Feedback Agent
 │               │
Redis Cache     Redis Feedback Store
 │
JSON Datasheets (authoritative source)
 │
Conversation State (in-memory)
</pre>

<h3>Components</h3>
<ul>
<li><b>Azure Function:</b> Single HTTP endpoint used to receive user messages.</li>
<li><b>Intent Orchestrator:</b> Uses Semantic Kernel + Azure OpenAI to classify requests (Question / Feedback).</li>
<li><b>Q&A Agent:</b> Extracts product + attribute, checks Redis cache, reads JSON datasheets if needed.</li>
<li><b>Feedback Agent:</b> Captures user corrections and stores them in Redis.</li>
<li><b>Conversation State:</b> Maintains last product, attribute, and answer.</li>
</ul>
</div>

<div class="section box">
<h2>How to Run</h2>

<h3>1. Clone Repository</h3>
<pre>
git clone &lt;repo-url&gt;
cd SKF.ProductAssistant
</pre>

<h3>2. Install Dependencies</h3>
<pre>
dotnet restore
</pre>

<h3>3. Run Azure Function</h3>
<pre>
func start
</pre>

<h3>4. Test API</h3>
<pre>
POST http://localhost:7071/api/AskProduct
</pre>
</div>

<div class="section box">
<h2>Required Environment Variables</h2>
<pre>
AOAI_ENDPOINT
AOAI_DEPLOYMENT
AOAI_API_KEY
REDIS_CONNECTION
USE_REDIS=true
</pre>
</div>

<div class="section box">
<h2>Caching Strategy</h2>
<pre>
User Question
   ↓
Redis Cache Check
   ↓
Cache HIT → return answer
Cache MISS → JSON lookup
   ↓
Save answer to Redis
</pre>
</div>

<div class="section box">
<h2>Hallucination Reduction</h2>
<ul>
<li>Answers only generated from local JSON datasheets.</li>
<li>LLM used only for extraction and classification.</li>
<li>If data not found → system abstains.</li>
</ul>
</div>

<div class="section box">
<h2>Example Interactions</h2>

<pre>
Q: What is the width of 6205?
A: The width of the 6205 bearing is 15 mm.

Q: And what about its diameter?
A: The diameter of the 6205 bearing is 52 mm.

Q: That last width is wrong—store my correction: 6205 width 15 mm.
A: Thanks—your feedback for 6205 / width has been saved.

Q: Diameter for 9999?
A: Sorry, I can’t find that information for ‘9999’.
</pre>

</div>

<div class="section box">
<h2>Technologies Used</h2>
<ul>
<li>C# / .NET 8</li>
<li>Azure Functions</li>
<li>Microsoft Semantic Kernel</li>
<li>Azure OpenAI</li>
<li>Redis Cache</li>
<li>Newtonsoft.Json</li>
</ul>
</div>

</body>
</html>
