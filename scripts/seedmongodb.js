const { MongoClient } = require('mongodb');

const url = 'mongodb://mongoadmin:mongoadmin@localhost:27017'; // Connection URL

const repositories = [
  {
    _id: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    Title: 'REPOSITORY 1',
    Description: 'Description for Repository 1'
  },
  {
    _id: '91494575-bff8-4c8e-8dac-8649059835ab',
    Title: 'REPOSITORY 2',
    Description: 'Description for Repository 2'
  }
];
const milestones = [
  {
    _id: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    Title: 'MILESTONE 1',
    Description: 'Description for Milestone 1',
    DueDate: '2023-12-1T12:00:00.000Z',
    State: 0,
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e'
  },
  {
    _id: '91494575-bff8-4c8e-8dac-8649059835ab',
    Title: 'MILESTONE 2',
    Description: 'Description for Milestone 2',
    DueDate: '2023-12-31T12:00:00.000Z',
    State: 0,
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e'
  }
];
const issues = [
  {
    Id: 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a',
    Title: 'Issue 1',
    Description: 'Description for Issue 1',
    CreatedAt: '2023-11-01T10:30:00.000Z',
    Assigne: 'assignee1@example.com',
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    MilestoneId: 'c4f09d2a-0b3a-4dbd-9a26-8a6a982c8123',
    TaskType: 1,
    Events: []
  },
  {
    Id: 'eafed591-3f43-43f8-b6c0-4ae0fb331cfb',
    Title: 'Issue 2',
    Description: 'Description for Issue 2',
    CreatedAt: '2023-11-01T14:45:00.000Z',
    Assigne: 'assignee2@example.com',
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    MilestoneId: '3b25df48-6575-4b9f-9c69-2782c0a45c4c',
    TaskType: 1,
    Events: []
  },
  {
    Id: 'b285f7e4-8762-4d33-8391-08ac6a92d97b',
    Title: 'Issue 3',
    Description: 'Description for Issue 3',
    CreatedAt: '2023-11-01T09:15:00.000Z',
    Assigne: 'assignee3@example.com',
    RepositoryId: '91494575-bff8-4c8e-8dac-8649059835ab',
    MilestoneId: '87aee5a3-4df7-4ed2-a0cf-68f5d48b7b2e',
    TaskType: 1,
    Events: []
  }
];
const branches = [
  {
    Id: '7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23',
    Name: 'feature-branch-1',
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e',
    IssueId: 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a'
  },
  {
    Id: 'b71a920d-dbbf-4a01-8b7d-b9b78413e9c4',
    Name: 'bugfix-branch-1',
    RepositoryId: '91494575-bff8-4c8e-8dac-8649059835ab',
    IssueId: 'eafed591-3f43-43f8-b6c0-4ae0fb331cfb'
  },
  {
    Id: '4a2b67b7-1049-40d8-9c84-fc2622ab8197',
    Name: 'develop',
    RepositoryId: 'b0d58598-8410-4fbf-bab8-46eab3afc34e'
  }
];
const pullRequests = [
  {
    Id: 'd1e3a5c6-2a9b-4a6f-97d3-1f38e639e688',
    Source: '7f9e8c84-5a64-4d8d-a6cb-0c01e89b4c23',
    Target: '4a2b67b7-1049-40d8-9c84-fc2622ab8197',
    IssueId: 'f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a',
    MilestoneId: 'c4f09d2a-0b3a-4dbd-9a26-8a6a982c8123',
    TaskType: 0,
    Events: []
  }
];
const comments = [
  {
    "Id": "a1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6",
    "DateTimeOccured": "2023-12-01T15:45:00.000Z",
    "EventType": 2,
    "Content": "This is the content of the comment for Task 1",
    "TaskId": "f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a"
  },
  {
    "Id": "b5c6d7e8-f9a0-b1c2-d3e4-f5a6b7c8d9e0",
    "DateTimeOccured": "2023-12-10T08:30:00.000Z",
    "EventType": 2,
    "Content": "Comment content for Task 2",
    "TaskId": "eafed591-3f43-43f8-b6c0-4ae0fb331cfb"
  },
  {
    "Id": "c1d2e3f4-a5b6-c7d8-e9f0-a1b2c3d4e5f6",
    "DateTimeOccured": "2023-12-20T12:15:00.000Z",
    "EventType": 2,
    "Content": "Another comment for Task 3",
    "TaskId": "b285f7e4-8762-4d33-8391-08ac6a92d97b"
  }
];
const labels = [
  {
    "Id": "a1b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6",
    "DateTimeOccured": "2023-12-01T15:45:00.000Z",
    "EventType": 1,
    "Name": "Label 1",
    "TaskId": "f4c7a2b9-74c5-4e9d-a0fb-2c963f12937a"
  }
];

async function seedData() {
  const client = new MongoClient(url);

  try {
    await client.connect();

    const db = client.db('GitHubSimulator');
    const issueCollection = db.collection('Issue');
    const repoCollection = db.collection('Repository');
    const milestoneCollection = db.collection('Milestone');
    const branchCollection = db.collection('Branch');
    const pullRequestCollection = db.collection('PullRequest');
    const commentCollection = db.collection('Comment');
    const labelCollection = db.collection('Label');
    
    await issueCollection.insertMany(issues);
    await repoCollection.insertMany(repositories);
    await milestoneCollection.insertMany(milestones);
    await branchCollection.insertMany(branches);
    await pullRequestCollection.insertMany(pullRequests);
    await commentCollection.insertMany(comments);
    await labelCollection.insertMany(labels);

    console.log('Data inserted successfully');
  } finally {
    client.close();
  }
}

seedData();
